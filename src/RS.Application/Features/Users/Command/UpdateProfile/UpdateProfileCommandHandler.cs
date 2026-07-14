using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;

namespace RS.Application.Features.Users.Commands.UpdateProfile;

public class UpdateProfileCommandHandler
    : IRequestHandler<UpdateProfileCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProfileCommandHandler(
        IUserRepository userRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateProfileCommand request,
        CancellationToken ct)
    {
        var user = await _userRepository.GetByIdAsync(
            _userContext.UserId,
            ct);

        if (user == null)
        {
            return Result.Failure(
                new Error(
                    "USER_NOT_FOUND",
                    "User not found."));
        }

        user.UpdateProfileSafe(
            request.FullName,
            request.Phone);

        await _userRepository.UpdateAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
