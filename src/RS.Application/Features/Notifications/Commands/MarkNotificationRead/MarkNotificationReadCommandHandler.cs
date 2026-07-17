using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;

namespace RS.Application.Features.Notifications.Commands.MarkNotificationRead;

public class MarkNotificationReadCommandHandler
    : IRequestHandler<
        MarkNotificationReadCommand,
        Result<bool>>
{

    private readonly INotificationRepository _repository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;


    public MarkNotificationReadCommandHandler(
        INotificationRepository repository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }



    public async Task<Result<bool>> Handle(
        MarkNotificationReadCommand request,
        CancellationToken ct)
    {

        var notification =
            await _repository.GetByIdAsync(
                request.NotificationId,
                ct);


        if (notification == null)
        {
            return Result<bool>.Failure(
                new Error(
                    "NOTIFICATION_NOT_FOUND",
                    "Notification not found."));
        }


        if (notification.UserId != _userContext.UserId)
        {
            return Result<bool>.Failure(
                new Error(
                    "FORBIDDEN",
                    "You cannot modify this notification."));
        }


        notification.IsRead = true;


        await _unitOfWork.SaveChangesAsync(ct);


        return Result<bool>.Success(true);
    }
}
