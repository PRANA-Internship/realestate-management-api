namespace RS.Domain.Entities;

public class PropertyImage
{
    public Guid Id { get; set; }

    public Guid PropertyId { get; set; }

    public string ImageUrl { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Property Property { get; set; } = default!;
}
