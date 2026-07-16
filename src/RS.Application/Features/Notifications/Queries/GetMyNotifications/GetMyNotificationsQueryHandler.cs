using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Notifications;
using RS.Domain.Common;

namespace RS.Application.Features.Notifications.Queries.GetMyNotifications;

public class GetMyNotificationsQueryHandler
    : IRequestHandler<
        GetMyNotificationsQuery,
        Result<IReadOnlyList<NotificationResponse>>>
{

    private readonly INotificationRepository _repository;
    private readonly IUserContext _userContext;


    public GetMyNotificationsQueryHandler(
        INotificationRepository repository,
        IUserContext userContext)
    {
        _repository = repository;
        _userContext = userContext;
    }


    public async Task<Result<IReadOnlyList<NotificationResponse>>> Handle(
        GetMyNotificationsQuery request,
        CancellationToken ct)
    {

        var notifications =
            await _repository.GetByUserAsync(
                _userContext.UserId,
                ct);


        var response = notifications
            .Select(x =>
                new NotificationResponse(
                    x.Id,
                    x.Title,
                    x.Message,
                    x.IsRead,
                    x.CreatedAt))
            .ToList();


        return Result<IReadOnlyList<NotificationResponse>>
            .Success(response);
    }
}
