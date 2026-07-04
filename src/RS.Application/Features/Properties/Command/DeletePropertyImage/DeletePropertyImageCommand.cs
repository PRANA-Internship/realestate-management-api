using MediatR;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Commands.DeletePropertyImage;

public record DeletePropertyImageCommand(Guid ImageId) : IRequest<Result>;
