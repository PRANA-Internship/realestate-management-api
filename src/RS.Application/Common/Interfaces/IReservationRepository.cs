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


    Task<Reservation?> GetByIdAsync(
    Guid reservationId,
    CancellationToken ct = default);

    Task<List<Reservation>> GetMyReservationsAsync(
        Guid buyerUserId,
        CancellationToken ct = default);

    Task<List<Reservation>> GetExpiredPendingReservationsAsync(
        CancellationToken ct = default);

    void Update(Reservation reservation);

    Task<IReadOnlyList<Reservation>> GetAllReservationAsync(
    Guid managerId,
    CancellationToken ct);
}
