using MediatR;
using RS.Contracts.Reservations;
using RS.Domain.Common;

namespace RS.Application.Features.Reservations.Queries.GetReservationById;

public record GetReservationByIdQuery(Guid ReservationId)
    : IRequest<Result<ReservationDetailResponse>>;
