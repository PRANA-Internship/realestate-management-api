using RS.Domain.Enums;

namespace RS.Domain.Entities;

public class Property
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public PropertyType Type { get; set; }
    public PropertyStatus Status { get; set; }

    public decimal Price { get; set; }

    public string Location { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Country { get; set; } = default!;

    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public double AreaSize { get; set; }

    public bool IsActive { get; set; } = true;

    public Guid CreatedByUserId { get; set; }
    public string CreatedByRole { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<PropertyImage> Images { get; set; }
        = new List<PropertyImage>();
}
