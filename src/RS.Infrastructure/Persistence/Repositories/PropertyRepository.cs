using Microsoft.EntityFrameworkCore;
using RS.Application.Common.Interfaces;
using RS.Domain.Entities;
using RS.Domain.Enums;

namespace RS.Infrastructure.Persistence.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly RSDbContext _dbContext;

    public PropertyRepository(RSDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Property property, CancellationToken ct = default)
    {
        await _dbContext.Properties.AddAsync(property, ct);

    }

    public async Task<List<Property>> GetAllAsync(
     string? city,
     decimal? minPrice,
     decimal? maxPrice,
     string? type,
     int page,
     int pageSize,
     CancellationToken ct = default)
    {
        var query = _dbContext.Properties
            .Include(p => p.Images)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(city))
        {
            query = query.Where(x => x.City.ToLower() == city.ToLower());
        }

        if (minPrice is decimal minimumPrice)
        {
            query = query.Where(x => x.Price >= minimumPrice);
        }

        if (maxPrice is decimal maximumPrice)
        {
            query = query.Where(x => x.Price <= maximumPrice);
        }

        if (!string.IsNullOrWhiteSpace(type) &&
            Enum.TryParse<PropertyType>(type, true, out var parsedType))
        {
            query = query.Where(x => x.Type == parsedType);
        }

        query = query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        return await query.ToListAsync(ct);
    }
}
