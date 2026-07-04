using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Commands.DeletePropertyImage;

public class DeletePropertyImageCommandHandler
    : IRequestHandler<DeletePropertyImageCommand, Result>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public DeletePropertyImageCommandHandler(
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
        DeletePropertyImageCommand request,
        CancellationToken ct)
    {
        var image = await _propertyRepository.GetOwnedImageAsync(
            request.ImageId,
            _userContext.UserId,
            ct);

        if (image is null)
        {
            return Result.Failure(
                new Error("PROPERTY_IMAGE_404", "Property image not found."));
        }

        if (image.Property.Images.Count == 1)
        {
            return Result.Failure(
                new Error("PROPERTY_IMAGE_001", "A property must have at least one image."));
        }

        await _storageService.DeleteImageAsync(image.ImageUrl, ct);

        _propertyRepository.RemoveImage(image);

        image.Property.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
