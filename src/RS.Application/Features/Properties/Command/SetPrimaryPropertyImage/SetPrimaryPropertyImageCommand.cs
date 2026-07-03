using MediatR;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Commands.SetPrimaryPropertyImage;

public record SetPrimaryPropertyImageCommand(Guid ImageId) : IRequest<Result>;
