using System.Runtime.InteropServices;
using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;
using RS.Domain.Enums;

namespace RS.Application.Features.Users.Commands.UpdateMySalesStatus;

public class UpdateMySalesStatusCommandHandler
    : IRequestHandler<UpdateMySalesStatusCommand, Result>
{

    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;


    public UpdateMySalesStatusCommandHandler(
        IUserRepository userRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork,
        INotificationService notificationSeervice)
    {
        _userRepository = userRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
        _notificationService = notificationSeervice;
    }



    public async Task<Result> Handle(
        UpdateMySalesStatusCommand request,
        CancellationToken ct)
    {

        if (_userContext.Role != UserRole.MANAGER)
        {
            return Result.Failure(
                new Error(
                    "FORBIDDEN",
                    "Only managers can update sales status."));
        }



        var sales =
            await _userRepository.GetSalesByManagerAndIdAsync(
                _userContext.UserId,
                request.SalesId,
                ct);



        if (sales == null)
        {
            return Result.Failure(
                new Error(
                    "SALES_NOT_FOUND",
                    "Sales user not found."));
        }



        sales.SetStatus(request.Status);


        await _userRepository.UpdateAsync(
            sales,
            ct);


        await _unitOfWork.SaveChangesAsync(ct);

        await _notificationService.NotifyAsync(_userContext.UserId,
            "Status Update",
            $"{sales.FullName} status updated to {request.Status}",
            ct);

        return Result.Success();
    }
}
