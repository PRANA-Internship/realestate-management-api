namespace RS.Contracts.Properties;

public record PublicPropertyDetailResponse(
    Guid Id,
    string Title,
    string Description,
    decimal Price,
    string PropertyType,
    string Status,
    bool CanReserve,
    decimal ReservationFee,
    int ReservationDurationHours,
    bool ReservationEnabled,
    string Country,
    string City,
    string Location,
    int Bedrooms,
    int Bathrooms,
    double AreaSize,
    IReadOnlyCollection<PropertyImageResponse> Images);
