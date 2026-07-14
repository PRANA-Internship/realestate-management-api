using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Properties;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Queries.GetPublicProperties;

public class GetPublicPropertiesQueryHandler
    : IRequestHandler<GetPublicPropertiesQuery, Result<PaginatedResult<PublicPropertyResponse>>>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IConfigurationService _configurationService;

    public GetPublicPropertiesQueryHandler(
        IPropertyRepository propertyRepository,
        IConfigurationService configurationService)
    {
        _propertyRepository = propertyRepository;
        _configurationService = configurationService;
    }

    public async Task<Result<PaginatedResult<PublicPropertyResponse>>> Handle(
        GetPublicPropertiesQuery request,
        CancellationToken ct)
    {
        var reservationFee = await _configurationService.GetDecimalAsync(
            "Reservation.Fee",
            ct);

        var properties = await _propertyRepository.GetPublicPropertiesAsync(
            request.City,
            request.MinPrice,
            request.MaxPrice,
            request.Type,
            request.Page,
            request.PageSize,
            ct);

        var response = properties.Data
            .Select(property => new PublicPropertyResponse(
                property.Id,
                property.Title,
                property.Price,
                property.Type.ToString(),
                property.Status == Domain.Enums.PropertyStatus.Available,
                reservationFee,
                property.Images
                    .FirstOrDefault(x => x.IsPrimary)
                    ?.ImageUrl))
            .ToList();

        return Result<PaginatedResult<PublicPropertyResponse>>
            .Success(new PaginatedResult<PublicPropertyResponse>(response, properties.Meta));
    }
}
