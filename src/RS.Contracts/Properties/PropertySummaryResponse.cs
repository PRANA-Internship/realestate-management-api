namespace RS.Contracts.Properties;

public record PropertySummaryResponse(
    Guid Id,
    string Title,
    string Type,
    string Status,
    decimal Price,
    string City,
    string? PrimaryImage
);
