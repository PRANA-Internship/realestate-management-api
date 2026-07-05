using Microsoft.EntityFrameworkCore;
using RS.Application.Common.Interfaces;
using RS.Domain.Entities;

namespace RS.Infrastructure.Persistence.Repositories;

public class ConfigurationRepository : IConfigurationRepository
{
    private readonly RSDbContext _dbContext;

    public ConfigurationRepository(RSDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SystemConfiguration?> GetByKeyAsync(
        string key,
        CancellationToken ct = default)
    {
        return await _dbContext.SystemConfigurations
            .FirstOrDefaultAsync(
                configuration => configuration.Key == key,
                ct);
    }

    public async Task<List<SystemConfiguration>> GetAllAsync(
        CancellationToken ct = default)
    {
        return await _dbContext.SystemConfigurations
            .OrderBy(configuration => configuration.Key)
            .ToListAsync(ct);
    }

    public async Task AddAsync(
        SystemConfiguration configuration,
        CancellationToken ct = default)
    {
        await _dbContext.SystemConfigurations
            .AddAsync(configuration, ct);
    }

    public void Update(SystemConfiguration configuration)
    {
        _dbContext.SystemConfigurations.Update(configuration);
    }
}
