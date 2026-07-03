namespace RS.Contracts.Properties;

public record PropertyResponse(
    Guid Id,
    string Title,
    string Description,
    string Type,
    string Status,
    decimal Price,
    string Location,
    string City,
    string Country,
    int Bedrooms,
    int Bathrooms,
    double AreaSize,
    IReadOnlyCollection<PropertyImageResponse> Images
);
