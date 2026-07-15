using RS.Application.Common.Interfaces;
using RS.Domain.Entities;

namespace RS.Infrastructure.Services;

public class NotificationService(
    INotificationRepository notificationRepository,
    INotificationSender notificationSender,
    IUnitOfWork unitOfWork)
    : INotificationService
{

    public async Task NotifyAsync(
        Guid userId,
        string title,
        string message,
        CancellationToken ct = default)
    {

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = title,
            Message = message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };


        await notificationRepository.AddAsync(
            notification,
            ct);


        await unitOfWork.SaveChangesAsync(ct);



        await notificationSender.SendAsync(
            userId,
            new
            {
                notification.Id,
                notification.Title,
                notification.Message,
                notification.CreatedAt
            },
            ct);
    }
}
