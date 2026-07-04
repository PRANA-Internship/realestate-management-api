using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;
using RS.Domain.Entities;

namespace RS.Application.Features.Properties.Commands.AddPropertyImages;

public class AddPropertyImagesCommandHandler
    : IRequestHandler<AddPropertyImagesCommand, Result>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public AddPropertyImagesCommandHandler(
        IPropertyRepository propertyRepository,
        IStorageService storageService,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _propertyRepository = propertyRepository;
        _storageService = storageService;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result> Handle(
        AddPropertyImagesCommand request,
        CancellationToken ct)
    {
        var property = await _propertyRepository.GetOwnedPropertyAsync(
            request.PropertyId,
            _userContext.UserId,
            ct);

        if (property is null)
        {
            return Result.Failure(
                new Error("PROPERTY_404", "Property not found."));
        }

        var displayOrder = property.Images.Count;

        foreach (var image in request.Images)
        {
            var imageUrl = await _storageService.UploadImageAsync(image, ct);

            property.Images.Add(new PropertyImage
            {
                Id = Guid.NewGuid(),
                PropertyId = property.Id,
                ImageUrl = imageUrl,
                IsPrimary = false,
                DisplayOrder = ++displayOrder,
                CreatedAt = DateTime.UtcNow
            });
        }

        property.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
