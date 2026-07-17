

using MediatR;
using RS.Domain.Common;

public record MarkNotificationReadCommand(
    Guid NotificationId)
    : IRequest<Result<bool>>;
