using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;
using RS.Domain.Enums;

namespace RS.Application.Features.Users.Commands.SetPassword;

public class SetPasswordCommandHandler
    : IRequestHandler<SetPasswordCommand, Result<bool>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public SetPasswordCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;

    }

    public async Task<Result<bool>> Handle(
        SetPasswordCommand request,
        CancellationToken ct)
    {

        var user = await _userRepository
            .GetByResetTokenAsync(request.Token, ct);

        if (user == null)
        {
            return Result<bool>.Failure(
                new Error("INVALID_TOKEN", "Invalid or expired token."));
        }


        if (user.PasswordResetExpiry < DateTime.UtcNow)
        {
            return Result<bool>.Failure(
                new Error("TOKEN_EXPIRED", "Reset token expired."));
        }


        var hashedPassword = _passwordHasher.Hash(request.NewPassword);


        user.ActivateWithPassword(hashedPassword);


        await _userRepository.UpdateAsync(user, ct);

        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true);
    }
}
