namespace RS.Contracts.Properties;

public record PropertyImageResponse(
    Guid Id,
    string ImageUrl,
    bool IsPrimary,
    int DisplayOrder
);
