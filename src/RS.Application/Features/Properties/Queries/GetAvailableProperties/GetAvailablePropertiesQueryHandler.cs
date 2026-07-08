using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Properties;
using RS.Domain.Common;
using RS.Domain.Enums;

namespace RS.Application.Features.Properties.Queries.GetAvailableProperties;


public class GetAvailablePropertiesQueryHandler
    : IRequestHandler<GetAvailablePropertiesQuery,
        Result<IReadOnlyList<PropertyResponse>>>
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



    public async Task<Result<IReadOnlyList<PropertyResponse>>> Handle(
        GetAvailablePropertiesQuery request,
        CancellationToken ct)
    {

        if (_userContext.Role != UserRole.SALES)
        {
            return Result<IReadOnlyList<PropertyResponse>>
                .Failure(
                new Error(
                    "FORBIDDEN",
                    "Only sales can access available properties."));
        }


        var sales = await _userRepository
            .GetByIdAsync(
                _userContext.UserId,
                ct);


        if (sales == null || sales.CreatedByUserId == null)
        {
            return Result<IReadOnlyList<PropertyResponse>>
                .Failure(
                new Error(
                    "MANAGER_NOT_FOUND",
                    "Sales manager was not found."));
        }


        var properties =
            await _propertyRepository
                .GetAvailableForSalesAsync(
                    sales.CreatedByUserId.Value,
                    ct);



        var response = properties
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



        return Result<IReadOnlyList<PropertyResponse>>
            .Success(response);
    }
}
