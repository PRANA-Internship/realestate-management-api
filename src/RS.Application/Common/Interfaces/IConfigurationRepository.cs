using RS.Domain.Entities;

namespace RS.Application.Common.Interfaces;

public interface IConfigurationRepository
{
    Task<SystemConfiguration?> GetByKeyAsync(
        string key,
        CancellationToken ct = default);

    Task<List<SystemConfiguration>> GetAllAsync(
        CancellationToken ct = default);

    Task AddAsync(
        SystemConfiguration configuration,
        CancellationToken ct = default);

    void Update(SystemConfiguration configuration);
}
