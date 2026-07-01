using MediatR;
using RS.Contracts.Auth;
using RS.Domain.Common;

namespace RS.Application.Features.Auth.Commands.Login
{
    public sealed record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponse>>;
}
