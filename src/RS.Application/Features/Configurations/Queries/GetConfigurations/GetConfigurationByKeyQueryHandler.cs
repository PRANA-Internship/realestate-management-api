using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Configurations;
using RS.Domain.Common;

namespace RS.Application.Features.Configurations.Queries.GetConfigurationByKey;

public class GetConfigurationByKeyQueryHandler
    : IRequestHandler<GetConfigurationByKeyQuery, Result<ConfigurationResponse>>
{
    private readonly IConfigurationRepository _configurationRepository;

    public GetConfigurationByKeyQueryHandler(
        IConfigurationRepository configurationRepository)
    {
        _configurationRepository = configurationRepository;
    }

    public async Task<Result<ConfigurationResponse>> Handle(
        GetConfigurationByKeyQuery request,
        CancellationToken ct)
    {
        var configuration = await _configurationRepository.GetByKeyAsync(
            request.Key,
            ct);

        if (configuration is null)
        {
            return Result<ConfigurationResponse>.Failure(
                new Error(
                    "CONFIGURATION_001",
                    "Configuration was not found."));
        }

        var response = new ConfigurationResponse(
            configuration.Id,
            configuration.Key,
            configuration.Value,
            configuration.Description,
            configuration.DataType.ToString(),
            configuration.DefaultValue
            );

        return Result<ConfigurationResponse>.Success(response);
    }
}
