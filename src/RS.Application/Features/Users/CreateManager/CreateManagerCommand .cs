using MediatR;
using RS.Domain.Common;

namespace RS.Application.Features.Users.Commands.CreateManager;

public class CreateManagerCommand : IRequest<Result<Guid>>
{
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
}
