using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Configurations;
using RS.Domain.Common;

namespace RS.Application.Features.Configurations.Queries.GetConfigurations;

public class GetConfigurationsQueryHandler
    : IRequestHandler<GetConfigurationsQuery,
        Result<IReadOnlyList<ConfigurationResponse>>>
{
    private readonly IConfigurationRepository _configurationRepository;

    public GetConfigurationsQueryHandler(
        IConfigurationRepository configurationRepository)
    {
        _configurationRepository = configurationRepository;
    }

    public async Task<Result<IReadOnlyList<ConfigurationResponse>>> Handle(
        GetConfigurationsQuery request,
        CancellationToken ct)
    {
        var configurations =
            await _configurationRepository.GetAllAsync(ct);

        var response = configurations
            .Select(configuration => new ConfigurationResponse(
                configuration.Id,
                configuration.Key,
                configuration.Value,
                configuration.Description,
                configuration.DataType.ToString(),
                configuration.DefaultValue))
            .ToList();

        return Result<IReadOnlyList<ConfigurationResponse>>
            .Success(response);
    }
}
