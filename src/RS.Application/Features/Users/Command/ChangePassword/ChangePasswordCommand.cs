using MediatR;
using RS.Domain.Common;


namespace RS.Application.Features.Users.Commands.ChangePassword;

public sealed record ChangePasswordCommand(
    string CurrentPassword,
    string NewPassword)
    : IRequest<Result>;
