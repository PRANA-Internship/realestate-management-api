using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace RS.Api.Hubs;

[Authorize]
public class NotificationHub : Hub
{
}
