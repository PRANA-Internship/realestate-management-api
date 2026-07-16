using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;

namespace RS.Application.Features.Users.Commands.ChangePassword;

public class ChangePasswordCommandHandler
    : IRequestHandler<ChangePasswordCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordCommandHandler(
        IUserRepository userRepository,
        IUserContext userContext,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _userContext = userContext;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }


    public async Task<Result> Handle(
        ChangePasswordCommand request,
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


        if (string.IsNullOrEmpty(user.PasswordHash))
        {
            return Result.Failure(
                new Error(
                    "PASSWORD_NOT_SET",
                    "Password has not been set."));
        }


        var validPassword = _passwordHasher.Verify(
            request.CurrentPassword,
            user.PasswordHash);


        if (!validPassword)
        {
            return Result.Failure(
                new Error(
                    "INVALID_PASSWORD",
                    "Current password is incorrect."));
        }


        var newPasswordHash = _passwordHasher.Hash(
            request.NewPassword);


        user.ChangePassword(newPasswordHash);


        await _userRepository.UpdateAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);


        return Result.Success();
    }
}
