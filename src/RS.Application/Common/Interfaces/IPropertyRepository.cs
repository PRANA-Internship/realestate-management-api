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


    Task<List<Property>> GetMyPropertiesAsync(
    Guid userId,
    int page,
    int pageSize,
    CancellationToken ct = default);

    Task<Property?> GetByIdAsync(
    Guid id,
    CancellationToken ct = default);

    Task<Property?> GetOwnedPropertyAsync(
    Guid propertyId,
    Guid userId,
    CancellationToken ct = default);

    Task<PropertyImage?> GetOwnedImageAsync(
    Guid imageId,
    Guid userId,
    CancellationToken ct = default);

    void Remove(Property property);

    void RemoveImage(PropertyImage image);

    Task<Property?> GetPropertyForUpdateAsync(Guid propertyId, CancellationToken ct = default);

    Task<List<Property>> GetPublicPropertiesAsync(
   string? city,
   decimal? minPrice,
   decimal? maxPrice,
   string? type,
   int page,
   int pageSize,
   CancellationToken ct = default);

    Task<Property?> GetPublicPropertyByIdAsync(
        Guid propertyId,
        CancellationToken ct = default);

    Task<IReadOnlyList<Property>> GetAvailableForSalesAsync(
    Guid managerId,
    CancellationToken ct);
}
