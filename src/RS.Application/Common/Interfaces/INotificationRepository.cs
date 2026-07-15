using RS.Domain.Entities;

namespace RS.Application.Common.Interfaces;

public interface INotificationRepository
{
    Task AddAsync(
        Notification notification,
        CancellationToken ct = default);


    Task<IReadOnlyList<Notification>> GetByUserAsync(
        Guid userId,
        CancellationToken ct = default);


    Task<Notification?> GetByIdAsync(
        Guid id,
        CancellationToken ct = default);
}
