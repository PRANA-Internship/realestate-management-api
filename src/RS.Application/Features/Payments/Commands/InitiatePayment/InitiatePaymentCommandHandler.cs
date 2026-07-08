using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RS.Application.Common.Interfaces;
using RS.Application.Common.Models;
using RS.Domain.Common;
using RS.Domain.Entities;
using RS.Domain.Enums;

namespace RS.Application.Features.Payments.Commands.InitiatePayment;

public class InitiatePaymentCommandHandler : IRequestHandler<InitiatePaymentCommand, Result<string>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IChapaService _chapaService;
    private readonly IUnitOfWork _unitOfWork;

    public InitiatePaymentCommandHandler(
        IReservationRepository reservationRepository,
        IPaymentRepository paymentRepository,
        IChapaService chapaService,
        IUnitOfWork unitOfWork)
    {
        _reservationRepository = reservationRepository;
        _paymentRepository = paymentRepository;
        _chapaService = chapaService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string>> Handle(InitiatePaymentCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _reservationRepository.GetByIdAsync(request.ReservationId, cancellationToken);
        if (reservation is null)
        {
            return Result<string>.Failure(new Error("RESERVATION_NOT_FOUND", "Reservation not found."));
        }

        if (reservation.Status != ReservationStatus.PendingPayment)
        {
            return Result<string>.Failure(new Error("INVALID_RESERVATION_STATUS", $"Cannot pay for a reservation that is {reservation.Status}."));
        }

        if (reservation.ExpiresAt <= DateTime.UtcNow)
        {
            reservation.Status = ReservationStatus.Expired;
            _reservationRepository.Update(reservation);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<string>.Failure(new Error("RESERVATION_EXPIRED", "Reservation has expired."));
        }

        var nameParts = reservation.BuyerFullName.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
        var firstName = nameParts.Length > 0 ? nameParts[0] : "Customer";
        var lastName = nameParts.Length > 1 ? nameParts[1] : "N/A";

        var txRef = $"TX-{reservation.Id}-{Guid.NewGuid().ToString().Substring(0, 8)}";

        var chapaRequest = new ChapaInitializeRequest
        {
            Amount = reservation.ReservationFee,
            Currency = "ETB",
            Email = reservation.BuyerEmail,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = reservation.BuyerPhoneNumber,
            TxRef = txRef,
            Customization = new ChapaCustomization
            {
                Title = "Property Reservation Fee",
                Description = $"Reservation Fee for Reservation {reservation.Id}"
            }
        };

        var response = await _chapaService.InitializePaymentAsync(chapaRequest, cancellationToken);

        if (response.Status != "success" || response.Data?.CheckoutUrl is null)
        {
            return Result<string>.Failure(new Error("PAYMENT_INITIALIZATION_FAILED", response.Message ?? "Failed to initialize payment with Chapa."));
        }

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            ReservationId = reservation.Id,
            TxRef = txRef,
            Amount = reservation.ReservationFee,
            Currency = "ETB",
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        await _paymentRepository.AddAsync(payment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success(response.Data.CheckoutUrl);
    }
}
