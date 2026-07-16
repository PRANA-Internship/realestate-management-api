
using RS.Domain.Enums;

public sealed class ProfileResponse
{
    public Guid Id { get; init; }

    public string FullName { get; init; } = default!;

    public string Email { get; init; } = default!;

    public string Phone { get; init; } = default!;

    public UserRole Role { get; init; }

    public UserStatus Status { get; init; }

    public DateTimeOffset CreatedAt { get; init; }
}
