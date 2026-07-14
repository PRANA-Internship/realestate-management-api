using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Properties;
using RS.Domain.Common;
using RS.Domain.Enums;

namespace RS.Application.Features.Properties.Queries.GetAvailableProperties;


public class GetAvailablePropertiesQueryHandler
    : IRequestHandler<GetAvailablePropertiesQuery,
        Result<PaginatedResult<PropertyResponse>>>
{

    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;
    private readonly IPropertyRepository _propertyRepository;


    public GetAvailablePropertiesQueryHandler(
        IUserContext userContext,
        IUserRepository userRepository,
        IPropertyRepository propertyRepository)
    {
        _userContext = userContext;
        _userRepository = userRepository;
        _propertyRepository = propertyRepository;
    }



    public async Task<Result<PaginatedResult<PropertyResponse>>> Handle(
        GetAvailablePropertiesQuery request,
        CancellationToken ct)
    {


        var sales = await _userRepository
            .GetByIdAsync(
                _userContext.UserId,
                ct);


        if (sales == null || sales.CreatedByUserId == null)
        {
            return Result<PaginatedResult<PropertyResponse>>
                .Failure(
                new Error(
                    "MANAGER_NOT_FOUND",
                    "Sales manager was not found."));
        }


        var properties =
            await _propertyRepository
                .GetAvailableForSalesAsync(
                    sales.CreatedByUserId.Value,
                    1, 10,
                    ct);



        var response = properties.Data
            .Select(x => new PropertyResponse(
                x.Id,
                x.Title,
                x.Description,
                x.Type.ToString(),
                x.Status.ToString(),
                x.Price,
                x.Location,
                x.City,
                x.Country,
                x.Bedrooms,
                x.Bathrooms,
                x.AreaSize,
                x.Images
                    .Select(i => new PropertyImageResponse(
                        i.Id,
                        i.ImageUrl,
                        i.IsPrimary,
                        i.DisplayOrder))
                    .ToList()
            ))
            .ToList();



        return Result<PaginatedResult<PropertyResponse>>
            .Success(new PaginatedResult<PropertyResponse>(response, properties.Meta));
    }
}
