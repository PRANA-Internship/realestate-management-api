using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Commands.DeleteProperty;

public class DeletePropertyCommandHandler
    : IRequestHandler<DeletePropertyCommand, Result>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public DeletePropertyCommandHandler(
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
        DeletePropertyCommand request,
        CancellationToken ct)
    {
        var property = await _propertyRepository.GetOwnedPropertyAsync(
            request.Id,
            _userContext.UserId,
            ct);

        if (property is null)
        {
            return Result.Failure(
                new Error("PROPERTY_404", "Property not found."));
        }

        foreach (var image in property.Images)
        {
            await _storageService.DeleteImageAsync(image.ImageUrl, ct);
        }

        _propertyRepository.Remove(property);

        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
