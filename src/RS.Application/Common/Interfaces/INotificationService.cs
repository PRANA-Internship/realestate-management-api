namespace RS.Application.Common.Interfaces;

public interface INotificationService
{
    Task NotifyAsync(
        Guid userId,
        string title,
        string message,
        CancellationToken ct = default);
}
