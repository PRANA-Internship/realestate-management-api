using MediatR;
using RS.Contracts.Reservations;
using RS.Domain.Common;

public class GetActiveReservationsQuery
    : IRequest<Result<IReadOnlyList<ReservationDetailResponse>>>
{
}
