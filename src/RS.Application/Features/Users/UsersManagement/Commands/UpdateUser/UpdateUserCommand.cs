using MediatR;
using RS.Domain.Common;

namespace RS.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<Result<bool>>
{
    public Guid UserId { get; set; }

    public string FullName { get; set; } = default!;

    public string Phone { get; set; } = default!;
}
