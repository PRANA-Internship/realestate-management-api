using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;
using RS.Domain.Enums;

namespace RS.Application.Features.Properties.Commands.UpdateProperty;

public class UpdatePropertyCommandHandler
    : IRequestHandler<UpdatePropertyCommand, Result>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly INotificationService _notificationService;
    public UpdatePropertyCommandHandler(
        IPropertyRepository propertyRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        INotificationService notificationService)
    {
        _propertyRepository = propertyRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _notificationService = notificationService;
    }

    public async Task<Result> Handle(
        UpdatePropertyCommand request,
        CancellationToken ct)
    {
        var property = await _propertyRepository.GetOwnedPropertyAsync(
            request.propertyId,
            _userContext.UserId,
            ct);

        if (property is null)
        {
            return Result.Failure(
                new Error("PROPERTY_404", "Property not found."));
        }

        property.Title = request.Title;
        property.Description = request.Description;
        property.Type = Enum.Parse<PropertyType>(request.Type, true);
        property.Status = Enum.Parse<PropertyStatus>(request.Status, true);
        property.Price = request.Price;
        property.Location = request.Location;
        property.City = request.City;
        property.Country = request.Country;
        property.Bedrooms = request.Bedrooms;
        property.Bathrooms = request.Bathrooms;
        property.AreaSize = request.AreaSize;
        property.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync(ct);
        await _notificationService.NotifyAsync(_userContext.UserId,
            "Property Update",
            "Property Update Successfull", ct);
        return Result.Success();
    }
}
