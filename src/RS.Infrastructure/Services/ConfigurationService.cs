using RS.Application.Common.Interfaces;

namespace RS.Infrastructure.Services;

public class ConfigurationService : IConfigurationService
{
    private readonly IConfigurationRepository _configurationRepository;

    public ConfigurationService(
        IConfigurationRepository configurationRepository)
    {
        _configurationRepository = configurationRepository;
    }

    public async Task<string?> GetValueAsync(
        string key,
        CancellationToken ct = default)
    {
        var configuration = await GetConfigurationAsync(key, ct);

        return configuration.Value;
    }

    public async Task<int> GetIntAsync(
        string key,
        CancellationToken ct = default)
    {
        var value = await GetValueAsync(key, ct);

        if (!int.TryParse(value, out var result))
        {
            throw new InvalidOperationException(
                $"Configuration '{key}' is not a valid integer.");
        }

        return result;
    }

    public async Task<decimal> GetDecimalAsync(
        string key,
        CancellationToken ct = default)
    {
        var value = await GetValueAsync(key, ct);

        if (!decimal.TryParse(value, out var result))
        {
            throw new InvalidOperationException(
                $"Configuration '{key}' is not a valid decimal.");
        }

        return result;
    }

    public async Task<bool> GetBooleanAsync(
        string key,
        CancellationToken ct = default)
    {
        var value = await GetValueAsync(key, ct);

        if (!bool.TryParse(value, out var result))
        {
            throw new InvalidOperationException(
                $"Configuration '{key}' is not a valid boolean.");
        }

        return result;
    }

    private async Task<RS.Domain.Entities.SystemConfiguration> GetConfigurationAsync(
        string key,
        CancellationToken ct)
    {
        var configuration = await _configurationRepository.GetByKeyAsync(key, ct);

        if (configuration is null)
        {
            throw new InvalidOperationException(
                $"Configuration '{key}' was not found.");
        }

        return configuration;
    }
}
