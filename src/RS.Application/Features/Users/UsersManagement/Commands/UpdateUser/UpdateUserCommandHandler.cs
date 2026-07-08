using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;
using RS.Domain.Enums;

namespace RS.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler
    : IRequestHandler<UpdateUserCommand, Result<bool>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;


    public UpdateUserCommandHandler(
        IUserRepository userRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }


    public async Task<Result<bool>> Handle(
        UpdateUserCommand request,
        CancellationToken ct)
    {

        // Admin only
        if (_userContext.Role != UserRole.ADMIN)
        {
            return Result<bool>.Failure(
                new Error(
                    "FORBIDDEN",
                    "Only admin can update users."));
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


        user.UpdateProfileSafe(
            request.FullName,
            request.Phone);


        await _userRepository.UpdateAsync(
            user,
            ct);


        await _unitOfWork.SaveChangesAsync(ct);


        return Result<bool>.Success(true);
    }
}
