using Microsoft.EntityFrameworkCore;
using RS.Application.Common.Interfaces;
using RS.Domain.Entities;

namespace RS.Infrastructure.Persistence.Repositories;

public class NotificationRepository(
    RSDbContext dbContext)
    : INotificationRepository
{

    public async Task AddAsync(
        Notification notification,
        CancellationToken ct = default)
    {
        await dbContext.Notifications
            .AddAsync(notification, ct);
    }


    public async Task<IReadOnlyList<Notification>> GetByUserAsync(
        Guid userId,
        CancellationToken ct = default)
    {
        return await dbContext.Notifications
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);
    }


    public async Task<Notification?> GetByIdAsync(
        Guid id,
        CancellationToken ct = default)
    {
        return await dbContext.Notifications
            .FirstOrDefaultAsync(
                x => x.Id == id,
                ct);
    }
}
