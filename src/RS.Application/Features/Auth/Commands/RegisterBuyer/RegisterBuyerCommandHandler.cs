using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;
using RS.Domain.Entities;

namespace RS.Application.Features.Auth.Commands.RegisterBuyer
{
    public sealed class RegisterBuyerCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        IEmailService emailService
    ) : IRequestHandler<RegisterBuyerCommand, Result>
    {
        public async Task<Result> Handle(RegisterBuyerCommand request, CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            if (await userRepository.ExistsByEmailAsync(email, cancellationToken))
            {
                return Error.AuthErrors.EmailAlreadyExists;
            }

            var passwordHash = passwordHasher.Hash(request.Password);

            var user = User.CreateBuyer(request.FullName, email, request.Phone, passwordHash);

            await userRepository.AddAsync(user, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Send confirmation/welcome email
            await emailService.SendWelcomeEmailAsync(user.Email, user.FullName, cancellationToken);

            return Result.Success();
        }
    }
}
