namespace RS.Contracts.Configurations;

public record ConfigurationResponse(
    Guid Id,
    string Key,
    string Value,
    string? Description,
    string DataType,      
    string DefaultValue
);
