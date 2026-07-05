namespace RS.Domain.Entities;

public class SystemConfiguration
{
    public Guid Id { get; set; }

    public string Key { get; set; } = default!;

    public string Value { get; set; } = default!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
