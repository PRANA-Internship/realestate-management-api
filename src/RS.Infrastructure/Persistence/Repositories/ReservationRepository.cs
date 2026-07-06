using Microsoft.EntityFrameworkCore;
using RS.Application.Common.Interfaces;
using RS.Domain.Entities;
using RS.Domain.Enums;

namespace RS.Infrastructure.Persistence.Repositories;

public class ReservationRepository(RSDbContext dbContext)
    : IReservationRepository
{
    public async Task AddAsync(
        Reservation reservation,
        CancellationToken ct = default)
    {
        await dbContext.Reservations.AddAsync(reservation, ct);
    }

    public async Task<int> CountActiveReservationsAsync(
        Guid buyerUserId,
        CancellationToken ct = default)
    {
        return await dbContext.Reservations.CountAsync(
            x =>
                x.BuyerUserId == buyerUserId &&
                (x.Status == ReservationStatus.PendingPayment ||
                 x.Status == ReservationStatus.Reserved),
            ct);
    }

    public async Task<int> CountActiveReservationsByEmailAsync(
        string email,
        CancellationToken ct = default)
    {
        return await dbContext.Reservations.CountAsync(
            x =>
                x.BuyerEmail == email &&
                (x.Status == ReservationStatus.PendingPayment ||
                 x.Status == ReservationStatus.Reserved),
            ct);
    }

    public async Task<bool> ExistsActiveReservationForPropertyAsync(
        Guid propertyId,
        CancellationToken ct = default)
    {
        return await dbContext.Reservations.AnyAsync(
            x =>
                x.PropertyId == propertyId &&
                (x.Status == ReservationStatus.PendingPayment ||
                 x.Status == ReservationStatus.Reserved),
            ct);
    }

    public async Task<Reservation?> GetByIdAsync(
    Guid reservationId,
    CancellationToken ct = default)
    {
        return await dbContext.Reservations
            .Include(x => x.Property)
            .ThenInclude(x => x.Images)
            .FirstOrDefaultAsync(
                x => x.Id == reservationId,
                ct);
    }

    public async Task<List<Reservation>> GetMyReservationsAsync(
    Guid buyerUserId,
    CancellationToken ct = default)
    {
        return await dbContext.Reservations
            .Include(x => x.Property)
            .ThenInclude(x => x.Images)
            .Where(x => x.BuyerUserId == buyerUserId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<List<Reservation>> GetExpiredPendingReservationsAsync(
    CancellationToken ct = default)
    {
        return await dbContext.Reservations
            .Where(x =>
                x.Status == ReservationStatus.PendingPayment &&
                x.ExpiresAt <= DateTime.UtcNow)
            .ToListAsync(ct);
    }

    public void Update(Reservation reservation)
    {
        dbContext.Reservations.Update(reservation);
    }
}
