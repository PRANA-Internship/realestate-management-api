using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;
using RS.Domain.Entities;
using RS.Domain.Enums;

namespace RS.Application.Features.Reservations.Commands.CreateReservation;

public class CreateReservationCommandHandler
    : IRequestHandler<CreateReservationCommand, Result<Guid>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IConfigurationService _configurationService;
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReservationCommandHandler(
        IReservationRepository reservationRepository,
        IPropertyRepository propertyRepository,
        IConfigurationService configurationService,
        IUserContext userContext,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _reservationRepository = reservationRepository;
        _propertyRepository = propertyRepository;
        _configurationService = configurationService;
        _userContext = userContext;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateReservationCommand request,
        CancellationToken ct)
    {

        var isEnabled = await _configurationService.GetBooleanAsync(
            "Reservation.Enabled", ct);

        if (!isEnabled)
        {
            return Result<Guid>.Failure(
                new Error("RESERVATION_DISABLED", "Reservations are currently disabled."));
        }

        var property = await _propertyRepository.GetPropertyForUpdateAsync(request.PropertyId, ct);
        if (property is null)
        {
            return Result<Guid>.Failure(
                new Error("PROPERTY_IN_ANOTHER_PROCESS", "Property is in another process.Try Again"));
        }

        if (!property.IsActive)
        {
            return Result<Guid>.Failure(
                new Error("PROPERTY_INACTIVE", "Property is inactive."));
        }

        if (property.Status != PropertyStatus.Available)
        {
            return Result<Guid>.Failure(
                new Error("PROPERTY_NOT_AVAILABLE", "Property cannot be reserved."));
        }


        var allowGuest = await _configurationService.GetBooleanAsync(
            "Reservation.AllowGuestReservation", ct);

        bool isAuthenticated = _userContext.UserId != Guid.Empty;

        string fullName;
        string email;
        string phone;

        if (isAuthenticated)
        {
            var user = await _userRepository.GetByIdAsync(_userContext.UserId, ct);

            if (user is null)
            {
                return Result<Guid>.Failure(
                    new Error("USER_NOT_FOUND", "User not found."));
            }

            fullName = user.FullName;
            email = user.Email;
            phone = user.Phone;
        }
        else
        {
            if (!allowGuest)
            {
                return Result<Guid>.Failure(
                    new Error("GUEST_NOT_ALLOWED", "Guest reservations are not allowed."));
            }

            if (string.IsNullOrWhiteSpace(request.BuyerFullName) ||
                string.IsNullOrWhiteSpace(request.BuyerEmail) ||
                string.IsNullOrWhiteSpace(request.BuyerPhoneNumber))
            {
                return Result<Guid>.Failure(
                    new Error("INVALID_GUEST_DATA", "Name, email and phone are required."));
            }

            fullName = request.BuyerFullName;
            email = request.BuyerEmail;
            phone = request.BuyerPhoneNumber;
        }


        email = email.Trim().ToLowerInvariant();


        var exists = await _reservationRepository
            .ExistsActiveReservationForPropertyAsync(property.Id, ct);

        if (exists)
        {
            return Result<Guid>.Failure(
                new Error("ALREADY_RESERVED", "This property already has an active reservation."));
        }

        var maxLimit = await _configurationService.GetIntAsync(
            "Reservation.MaximumPerBuyer", ct);

        int activeCount = isAuthenticated
            ? await _reservationRepository.CountActiveReservationsAsync(_userContext.UserId, ct)
            : await _reservationRepository.CountActiveReservationsByEmailAsync(email, ct);

        if (activeCount >= maxLimit)
        {
            return Result<Guid>.Failure(
                new Error("RESERVATION_LIMIT_REACHED", "Maximum active reservations reached."));
        }


        var fee = await _configurationService.GetDecimalAsync("Reservation.Fee", ct);
        var durationHours = await _configurationService.GetIntAsync("Reservation.DurationHours", ct);

        var now = DateTime.UtcNow;


        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            PropertyId = property.Id,

            BuyerUserId = isAuthenticated ? _userContext.UserId : null,

            BuyerFullName = fullName,
            BuyerEmail = email,
            BuyerPhoneNumber = phone,

            ReservationFee = fee,
            Status = ReservationStatus.PendingPayment,

            ReservedAt = now,
            ExpiresAt = now.AddHours(durationHours),

            CreatedAt = now
        };


        await _reservationRepository.AddAsync(reservation, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<Guid>.Success(reservation.Id);
    }
}
