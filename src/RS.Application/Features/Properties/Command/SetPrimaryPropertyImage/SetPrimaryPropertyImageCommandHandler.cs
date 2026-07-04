using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Commands.SetPrimaryPropertyImage;

public class SetPrimaryPropertyImageCommandHandler
    : IRequestHandler<SetPrimaryPropertyImageCommand, Result>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public SetPrimaryPropertyImageCommandHandler(
        IPropertyRepository propertyRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _propertyRepository = propertyRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result> Handle(
        SetPrimaryPropertyImageCommand request,
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

        foreach (var propertyImage in image.Property.Images)
        {
            propertyImage.IsPrimary = false;
        }

        image.IsPrimary = true;
        image.Property.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
