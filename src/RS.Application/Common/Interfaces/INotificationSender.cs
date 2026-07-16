namespace RS.Application.Common.Interfaces;

public interface INotificationSender
{
    Task SendAsync(
        Guid userId,
        object notification,
        CancellationToken ct = default);
}
