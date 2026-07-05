using MediatR;
using RS.Contracts.Reservations;
using RS.Domain.Common;

namespace RS.Application.Features.Reservations.Queries.GetMyReservations;

public record GetMyReservationsQuery()
    : IRequest<Result<IReadOnlyCollection<ReservationResponse>>>;
