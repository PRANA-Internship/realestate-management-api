using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Auth;
using RS.Domain.Common;

namespace RS.Application.Features.Auth.Commands.Login
{
    public sealed class LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService
    ) : IRequestHandler<LoginCommand, Result<AuthResponse>>
    {
        public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var user = await userRepository.GetByEmailAsync(email, cancellationToken);
            var hash = user?.PasswordHash ?? "$2a$12$invalidhashpaddingtomakeconstanttime";

            if (user is null || !passwordHasher.Verify(request.Password, hash))
            {
                return Error.AuthErrors.InvalidCredentials;
            }

            if (user.Status == RS.Domain.Enums.UserStatus.INACTIVE)
            {
                return Error.AuthErrors.UserInactive;
            }

            var accessToken = tokenService.GenerateAccessToken(user);

            return Result<AuthResponse>.Success(new AuthResponse(
                AccessToken: accessToken,
                Email: user.Email,
                FullName: user.FullName,
                Role: user.Role.ToString()
            ));
        }
    }
}
