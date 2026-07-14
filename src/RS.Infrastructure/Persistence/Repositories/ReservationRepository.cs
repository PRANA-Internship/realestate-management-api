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

    public async Task<IReadOnlyList<Reservation>> GetAllReservationAsync(
    Guid managerId,
    CancellationToken ct)
    {
        return await dbContext.Reservations
            .Include(r => r.Property)
            .Where(r =>
                r.Property.CreatedByUserId == managerId &&
                r.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(r => r.ReservedAt)
            .ToListAsync(ct);
    }

    public async Task<PaginatedResult<Reservation>> GetManagedPropertyReservationsAsync(Guid managerId, ReservationStatus? status,
        string? search,
        int page,
        int pageSize,
        CancellationToken ct = default)
    {

        var query = dbContext.Reservations.Include(x => x.Property)
            .Where(x => x.Property.CreatedByUserId == managerId).AsQueryable();

        if (status.HasValue)
        {

            query = query.Where(x => x.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {

            query = query.Where(x =>
            x.BuyerFullName.ToLower().Contains(search) ||
            x.BuyerEmail.ToLower().Contains(search) ||
            x.BuyerPhoneNumber.Contains(search));
        }

        var totalItems = await query.CountAsync(ct);

        var reservations = await query.OrderByDescending(x => x.ReservedAt).Skip((page - 1) * pageSize)
            .Take(pageSize).ToListAsync(ct);

        return new PaginatedResult<Reservation>(reservations, new PaginationMetadata(
            page,
            pageSize,
            totalItems));


    }
}
