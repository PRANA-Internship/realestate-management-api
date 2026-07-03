using MediatR;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Commands.UpdateProperty;

public class UpdatePropertyCommand : IRequest<Result>
{
    public Guid propertyId { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Type { get; set; } = default!;

    public string Status { get; set; } = default!;

    public decimal Price { get; set; }

    public string Location { get; set; } = default!;

    public string City { get; set; } = default!;

    public string Country { get; set; } = default!;

    public int Bedrooms { get; set; }

    public int Bathrooms { get; set; }

    public double AreaSize { get; set; }
}
