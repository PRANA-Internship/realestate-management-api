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

    public async Task<List<Property>> GetMyPropertiesAsync(
    Guid userId,
    int page,
    int pageSize,
    CancellationToken ct = default)
    {
        return await _dbContext.Properties
            .Include(p => p.Images)
            .Where(p => p.CreatedByUserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<Property?> GetByIdAsync(
    Guid id,
    CancellationToken ct = default)
    {
        return await _dbContext.Properties
            .Include(p => p.Images)
            .FirstOrDefaultAsync(
                p => p.Id == id,
                ct);
    }

    public async Task<Property?> GetOwnedPropertyAsync(
    Guid propertyId,
    Guid userId,
    CancellationToken ct = default)
    {
        return await _dbContext.Properties
            .Include(p => p.Images)
            .FirstOrDefaultAsync(
                p => p.Id == propertyId &&
                     p.CreatedByUserId == userId,
                ct);


    }


    public void Remove(Property property)
    {
        _dbContext.Properties.Remove(property);
    }


    public async Task<PropertyImage?> GetOwnedImageAsync(
    Guid imageId,
    Guid userId,
    CancellationToken ct = default)
    {
        return await _dbContext.PropertyImages
            .Include(i => i.Property)
            .Include(i => i.Property.Images)
            .FirstOrDefaultAsync(
                i => i.Id == imageId &&
                     i.Property.CreatedByUserId == userId,
                ct);
    }

    public void RemoveImage(PropertyImage image)
    {
        _dbContext.PropertyImages.Remove(image);
    }


    public async Task<Property?> GetPropertyForUpdateAsync(Guid propertyId, CancellationToken ct = default)
    {
        return await _dbContext.Properties
            .FromSqlInterpolated($@"
            SELECT * FROM ""Properties""
            WHERE ""Id"" = {propertyId}
            FOR UPDATE
        ")
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<Property>> GetPublicPropertiesAsync(
    string? city,
    decimal? minPrice,
    decimal? maxPrice,
    string? type,
    int page,
    int pageSize,
    CancellationToken ct = default)
    {
        var query = _dbContext.Properties
            .Include(x => x.Images)
            .Where(x =>
                x.IsActive &&
                x.Status == PropertyStatus.Available)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(city))
        {
            query = query.Where(x =>
                x.City.ToLower() == city.ToLower());
        }

        if (minPrice is decimal min)
        {
            query = query.Where(x => x.Price >= min);
        }

        if (maxPrice is decimal max)
        {
            query = query.Where(x => x.Price <= max);
        }

        if (!string.IsNullOrWhiteSpace(type) &&
            Enum.TryParse<PropertyType>(
                type,
                true,
                out var propertyType))
        {
            query = query.Where(x =>
                x.Type == propertyType);
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<Property?> GetPublicPropertyByIdAsync(
    Guid propertyId,
    CancellationToken ct = default)
    {
        return await _dbContext.Properties
            .Include(x => x.Images)
            .FirstOrDefaultAsync(
                x =>
                    x.Id == propertyId &&
                    x.IsActive &&
                    x.Status == PropertyStatus.Available,
                ct);
    }

    public async Task<IReadOnlyList<Property>> GetAvailableForSalesAsync(
    Guid managerId,
    CancellationToken ct)
    {
        return await _dbContext.Properties
            .Where(x =>
                x.CreatedByUserId == managerId &&
                x.IsActive)
            .Include(x => x.Images)
            .ToListAsync(ct);
    }
}
