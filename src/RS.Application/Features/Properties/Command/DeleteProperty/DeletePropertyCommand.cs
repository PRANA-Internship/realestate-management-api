using MediatR;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Commands.DeleteProperty;

public record DeletePropertyCommand(Guid Id) : IRequest<Result>;
