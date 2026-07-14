using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using RS.Contracts.Reservations;
using RS.Domain.Common;
using RS.Domain.Enums;

namespace RS.Application.Features.Reservations.Queries.GetManagedPropertyReservation
{
    public class GetManagedPropertyReservationsQuery : IRequest<Result<PaginatedResult<ManagedPropertyReservationResponse>>>
    {
        public ReservationStatus? Status { get; init; }
        public string? Search { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
