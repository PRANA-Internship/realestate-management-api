using RS.Domain.Entities;

namespace RS.Application.Common.Interfaces;

public interface IReservationRepository
{
    Task AddAsync(
        Reservation reservation,
        CancellationToken ct = default);

    Task<int> CountActiveReservationsAsync(
        Guid buyerUserId,
        CancellationToken ct = default);

    Task<int> CountActiveReservationsByEmailAsync(
        string email,
        CancellationToken ct = default);

    Task<bool> ExistsActiveReservationForPropertyAsync(
        Guid propertyId,
        CancellationToken ct = default);


}
