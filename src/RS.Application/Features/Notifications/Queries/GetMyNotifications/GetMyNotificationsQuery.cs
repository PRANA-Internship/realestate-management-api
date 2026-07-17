
using MediatR;
using RS.Contracts.Notifications;
using RS.Domain.Common;

public record GetMyNotificationsQuery()
    : IRequest<Result<IReadOnlyList<NotificationResponse>>>;
