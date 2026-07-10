using MediatR;
using RS.Domain.Common;
using RS.Domain.Enums;

namespace RS.Application.Features.Users.Commands.UpdateUserStatus;

public record UpdateUserStatusCommand(
    Guid UserId,
    UserStatus Status
) : IRequest<Result<bool>>;
