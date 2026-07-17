using Microsoft.AspNetCore.SignalR;
using RS.Api.Hubs;
using RS.Application.Common.Interfaces;

namespace RS.Api.Services;

public class SignalRNotificationSender(
    IHubContext<NotificationHub> hubContext)
    : INotificationSender
{

    public async Task SendAsync(
        Guid userId,
        object notification,
        CancellationToken ct = default)
    {

        await hubContext.Clients
            .User(userId.ToString())
            .SendAsync(
                "ReceiveNotification",
                notification,
                ct);
    }
}
