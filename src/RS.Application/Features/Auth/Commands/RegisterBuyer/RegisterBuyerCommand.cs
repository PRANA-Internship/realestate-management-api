using MediatR;
using RS.Domain.Common;

namespace RS.Application.Features.Auth.Commands.RegisterBuyer
{
    public sealed record RegisterBuyerCommand(string FullName, string Email, string Phone, string Password) : IRequest<Result>;
}
