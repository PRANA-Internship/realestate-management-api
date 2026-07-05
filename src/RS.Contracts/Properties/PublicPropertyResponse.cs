namespace RS.Contracts.Properties;

public record PublicPropertyResponse(
    Guid Id,
    string Title,
    decimal Price,
    string PropertyType,
    bool CanReserve,
    decimal ReservationFee,
    string? PrimaryImageUrl);
