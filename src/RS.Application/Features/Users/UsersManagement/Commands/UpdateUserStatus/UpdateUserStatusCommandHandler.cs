using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;
using RS.Domain.Enums;

namespace RS.Application.Features.Users.Commands.UpdateUserStatus;

public class UpdateUserStatusCommandHandler
    : IRequestHandler<UpdateUserStatusCommand, Result<bool>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;


    public UpdateUserStatusCommandHandler(
        IUserRepository userRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }


    public async Task<Result<bool>> Handle(
        UpdateUserStatusCommand request,
        CancellationToken ct)
    {

        if (_userContext.Role != UserRole.ADMIN)
        {
            return Result<bool>.Failure(
                new Error(
                    "FORBIDDEN",
                    "Only admin can update user status."));
        }


        var user = await _userRepository.GetByIdAsync(
            request.UserId,
            ct);


        if (user == null)
        {
            return Result<bool>.Failure(
                new Error(
                    "USER_NOT_FOUND",
                    "User was not found."));
        }


        // Prevent invalid status changes if needed later
        if (user.Status == request.Status)
        {
            return Result<bool>.Failure(
                new Error(
                    "STATUS_ALREADY_SET",
                    "User already has this status."));
        }


        user.SetStatus(request.Status);


        await _userRepository.UpdateAsync(
            user,
            ct);


        await _unitOfWork.SaveChangesAsync(ct);


        return Result<bool>.Success(true);
    }
}
