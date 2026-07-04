using MediatR;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Commands.ChangePropertyActiveState;

public class ChangePropertyActiveStateCommand : IRequest<Result>
{
    public Guid PropertyId { get; set; }

    public bool IsActive { get; set; }
}
