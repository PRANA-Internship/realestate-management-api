using MediatR;
using RS.Contracts.Reservations;
using RS.Domain.Common;
namespace RS.Application.Features.Reservations.Queries.GetActiveReservations;

public class GetActiveReservationsQuery
    : IRequest<Result<IReadOnlyList<ReservationDetailResponse>>>
{
}
