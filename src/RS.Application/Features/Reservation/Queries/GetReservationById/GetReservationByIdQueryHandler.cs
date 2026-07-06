using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Reservations;
using RS.Domain.Common;
using RS.Domain.Enums;

namespace RS.Application.Features.Reservations.Queries.GetReservationById;

public class GetReservationByIdQueryHandler
    : IRequestHandler<GetReservationByIdQuery, Result<ReservationDetailResponse>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserContext _userContext;

    public GetReservationByIdQueryHandler(
        IReservationRepository reservationRepository,
        IUserContext userContext)
    {
        _reservationRepository = reservationRepository;
        _userContext = userContext;
    }

    public async Task<Result<ReservationDetailResponse>> Handle(
        GetReservationByIdQuery request,
        CancellationToken ct)
    {
        var reservation = await _reservationRepository.GetByIdAsync(
            request.ReservationId,
            ct);

        if (reservation is null)
        {
            return Result<ReservationDetailResponse>.Failure(
                new Error(
                    "RESERVATION_NOT_FOUND",
                    "Reservation was not found."));
        }


        bool isPrivileged = _userContext.Role == UserRole.ADMIN || _userContext.Role == UserRole.MANAGER;
        bool isOwner = reservation.BuyerUserId == _userContext.UserId;

        if (!isPrivileged && !isOwner)
        {
            return Result<ReservationDetailResponse>.Failure(
                new Error(
                    "FORBIDDEN",
                    "You do not have permission to view this reservation."));
        }

        var response = new ReservationDetailResponse(
            reservation.Id,
            reservation.PropertyId,
            reservation.Property.Title,
            reservation.Property.Price,

            reservation.BuyerFullName,
            reservation.BuyerEmail,
            reservation.BuyerPhoneNumber,

            reservation.ReservationFee,
            reservation.Status.ToString(),

            reservation.ReservedAt,
            reservation.ExpiresAt);

        return Result<ReservationDetailResponse>.Success(response);
    }
}
