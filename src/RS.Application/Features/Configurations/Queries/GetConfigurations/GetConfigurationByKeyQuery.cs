using MediatR;
using RS.Contracts.Configurations;
using RS.Domain.Common;

namespace RS.Application.Features.Configurations.Queries.GetConfigurationByKey;

public record GetConfigurationByKeyQuery(string Key)
    : IRequest<Result<ConfigurationResponse>>;
