namespace RS.Contracts.Users;

public record UserDetailResponse(
    Guid Id,
    string FullName,
    string Email,
    string Phone,
    string Role,
    string Status,
    Guid? CreatedByUserId,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);
