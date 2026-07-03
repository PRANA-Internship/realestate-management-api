using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Properties;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Queries.GetPropertyById;

public class GetPropertyByIdQueryHandler
    : IRequestHandler<GetPropertyByIdQuery, Result<PropertyResponse>>
{
    private readonly IPropertyRepository _propertyRepository;

    public GetPropertyByIdQueryHandler(IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository;
    }

    public async Task<Result<PropertyResponse>> Handle(
        GetPropertyByIdQuery request,
        CancellationToken ct)
    {
        var property = await _propertyRepository.GetByIdAsync(request.Id, ct);

        if (property is null)
        {
            return Result<PropertyResponse>.Failure(
                new Error("PROPERTY_404", "Property not found."));
        }

        var response = new PropertyResponse(
            property.Id,
            property.Title,
            property.Description,
            property.Type.ToString(),
            property.Status.ToString(),
            property.Price,
            property.Location,
            property.City,
            property.Country,
            property.Bedrooms,
            property.Bathrooms,
            property.AreaSize,
            property.Images
                .OrderBy(i => i.DisplayOrder)
                .Select(i => new PropertyImageResponse(
                    i.Id,
                    i.ImageUrl,
                    i.IsPrimary,
                    i.DisplayOrder))
                .ToList());

        return Result<PropertyResponse>.Success(response);
    }
}
