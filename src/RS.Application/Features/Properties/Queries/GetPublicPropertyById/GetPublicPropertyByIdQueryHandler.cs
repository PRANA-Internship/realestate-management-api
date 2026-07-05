using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Properties;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Queries.GetPublicPropertyById;

public class GetPublicPropertyByIdQueryHandler
    : IRequestHandler<GetPublicPropertyByIdQuery, Result<PublicPropertyDetailResponse>>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IConfigurationService _configurationService;

    public GetPublicPropertyByIdQueryHandler(
        IPropertyRepository propertyRepository,
        IConfigurationService configurationService)
    {
        _propertyRepository = propertyRepository;
        _configurationService = configurationService;
    }

    public async Task<Result<PublicPropertyDetailResponse>> Handle(
        GetPublicPropertyByIdQuery request,
        CancellationToken ct)
    {
        var property = await _propertyRepository.GetPublicPropertyByIdAsync(
            request.PropertyId,
            ct);

        if (property is null)
        {
            return Result<PublicPropertyDetailResponse>.Failure(
                new Error(
                    "PROPERTY_NOT_FOUND",
                    "Property was not found."));
        }

        var reservationFee = await _configurationService.GetDecimalAsync(
            "Reservation.Fee",
            ct);

        var reservationDuration = await _configurationService.GetIntAsync(
            "Reservation.DurationHours",
            ct);

        var reservationEnabled = await _configurationService.GetBooleanAsync(
            "Reservation.Enabled",
            ct);

        var response = new PublicPropertyDetailResponse(
            property.Id,
            property.Title,
            property.Description,
            property.Price,
            property.Type.ToString(),
            property.Status.ToString(),
            property.Status == Domain.Enums.PropertyStatus.Available,
            reservationFee,
            reservationDuration,
            reservationEnabled,
            property.Country,
            property.City,
            property.Location,
            property.Bedrooms,
            property.Bathrooms,
            property.AreaSize,
            property.Images
                .Select(image => new PropertyImageResponse(
                    image.Id,
                    image.ImageUrl,
                    image.IsPrimary,
                    image.DisplayOrder))
                .ToList());

        return Result<PublicPropertyDetailResponse>.Success(response);
    }
}
