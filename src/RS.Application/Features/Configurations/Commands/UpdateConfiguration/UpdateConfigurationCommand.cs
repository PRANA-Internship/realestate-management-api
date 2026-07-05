using MediatR;
using RS.Domain.Common;

namespace RS.Application.Features.Configurations.Commands.UpdateConfiguration;

public record UpdateConfigurationCommand(
    string Key,
    string Value)
    : IRequest<Result>;
