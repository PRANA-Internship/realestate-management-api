using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Properties;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Queries.GetMyProperties;

public class GetMyPropertiesQueryHandler
    : IRequestHandler<GetMyPropertiesQuery, Result<IReadOnlyList<PropertySummaryResponse>>>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IUserContext _userContext;

    public GetMyPropertiesQueryHandler(
        IPropertyRepository propertyRepository,
        IUserContext userContext)
    {
        _propertyRepository = propertyRepository;
        _userContext = userContext;
    }

    public async Task<Result<IReadOnlyList<PropertySummaryResponse>>> Handle(
    GetMyPropertiesQuery request,
    CancellationToken ct)
    {
        var properties = await _propertyRepository.GetMyPropertiesAsync(
            _userContext.UserId,
            request.Page,
            request.PageSize,
            ct);

        var response = properties.Data
            .Select(property => new PropertySummaryResponse(
                property.Id,
                property.Title,
                property.Type.ToString(),
                property.Status.ToString(),
                property.Price,
                property.City,
                property.Images
                    .FirstOrDefault(image => image.IsPrimary)?
                    .ImageUrl))
            .ToList();

        return Result<IReadOnlyList<PropertySummaryResponse>>.Success(response);
    }
}
