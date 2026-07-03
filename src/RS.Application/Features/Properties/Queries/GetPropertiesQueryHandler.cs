using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Properties;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Queries.GetProperties;

public class GetPropertiesQueryHandler
    : IRequestHandler<GetPropertiesQuery, Result<List<PropertyResponse>>>
{
    private readonly IPropertyRepository _propertyRepository;

    public GetPropertiesQueryHandler(IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository;
    }

    public async Task<Result<List<PropertyResponse>>> Handle(
        GetPropertiesQuery request,
        CancellationToken ct)
    {
        var properties = await _propertyRepository.GetAllAsync(
            request.City,
            request.MinPrice,
            request.MaxPrice,
            request.Type,
            request.Page,
            request.PageSize,
            ct);
        var result = properties.Select(p => new PropertyResponse(
            p.Id,
            p.Title,
            p.Description,
            p.Type.ToString(),
            p.Status.ToString(),
            p.Price,
            p.Location,
            p.City,
            p.Country,
            p.Bedrooms,
            p.Bathrooms,
            p.AreaSize,
            p.Images.Select(i => new PropertyImageResponse(
                i.Id,
                i.ImageUrl,
                i.IsPrimary,
                i.DisplayOrder
            )).ToList()
        )).ToList();

        return Result<List<PropertyResponse>>.Success(result);
    }
}
