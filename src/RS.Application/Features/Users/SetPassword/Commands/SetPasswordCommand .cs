using MediatR;
using RS.Domain.Common;

namespace RS.Application.Features.Users.Commands.SetPassword;

public class SetPasswordCommand : IRequest<Result<bool>>
{
    public string Token { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}
