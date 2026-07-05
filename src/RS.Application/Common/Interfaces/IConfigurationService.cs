namespace RS.Application.Common.Interfaces;

public interface IConfigurationService
{
    Task<string?> GetValueAsync(
        string key,
        CancellationToken ct = default);

    Task<int> GetIntAsync(
        string key,
        CancellationToken ct = default);

    Task<decimal> GetDecimalAsync(
        string key,
        CancellationToken ct = default);

    Task<bool> GetBooleanAsync(
        string key,
        CancellationToken ct = default);
}
