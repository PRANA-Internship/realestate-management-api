using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;
using RS.Domain.Enums;

namespace RS.Application.Features.Users.Commands.UpdateMySalesStatus;

public class UpdateMySalesStatusCommand
    : IRequest<Result>
{
    public Guid SalesId { get; set; }

    public UserStatus Status { get; set; }
}
