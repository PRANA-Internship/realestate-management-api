namespace RS.Contracts.Users;

public record UserResponse(
    Guid Id,
    string FullName,
    string Email,
    string Phone,
    string Role,
    string Status,
    DateTimeOffset CreatedAt);
