using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Reservations;
using RS.Domain.Common;

namespace RS.Application.Features.Reservations.Queries.GetMyReservations;

public class GetMyReservationsQueryHandler
    : IRequestHandler<GetMyReservationsQuery, Result<IReadOnlyCollection<ReservationResponse>>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserContext _userContext;

    public GetMyReservationsQueryHandler(
        IReservationRepository reservationRepository,
        IUserContext userContext)
    {
        _reservationRepository = reservationRepository;
        _userContext = userContext;
    }

    public async Task<Result<IReadOnlyCollection<ReservationResponse>>> Handle(
        GetMyReservationsQuery request,
        CancellationToken ct)
    {
        var reservations = await _reservationRepository.GetMyReservationsAsync(
            _userContext.UserId,
            ct);

        var response = reservations
            .Select(x => new ReservationResponse(
                x.Id,
                x.PropertyId,
                x.Property.Title,
                x.Property.Price,
                x.ReservationFee,
                x.Status.ToString(),
                x.ReservedAt,
                x.ExpiresAt))
            .ToList();

        return Result<IReadOnlyCollection<ReservationResponse>>
            .Success(response);
    }
}
