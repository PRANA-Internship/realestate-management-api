using RS.Domain.Entities;

namespace RS.Application.Common.Interfaces;

public interface IPropertyRepository
{
    Task AddAsync(Property property, CancellationToken ct = default);
    Task<List<Property>> GetAllAsync(
        string? city,
        decimal? minPrice,
        decimal? maxPrice,
        string? type,
        int page,
        int pageSize,
        CancellationToken ct = default);

    Task<Property?> GetByIdAsync(
    Guid id,
    CancellationToken ct = default);
}
