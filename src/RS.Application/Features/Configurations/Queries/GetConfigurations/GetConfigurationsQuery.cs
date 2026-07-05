using MediatR;
using RS.Contracts.Configurations;
using RS.Domain.Common;

namespace RS.Application.Features.Configurations.Queries.GetConfigurations;

public record GetConfigurationsQuery()
    : IRequest<Result<IReadOnlyList<ConfigurationResponse>>>;
