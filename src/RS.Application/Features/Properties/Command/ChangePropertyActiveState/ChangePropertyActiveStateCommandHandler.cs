using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Commands.ChangePropertyActiveState;

public class ChangePropertyActiveStateCommandHandler
    : IRequestHandler<ChangePropertyActiveStateCommand, Result>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public ChangePropertyActiveStateCommandHandler(
        IPropertyRepository propertyRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _propertyRepository = propertyRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result> Handle(
        ChangePropertyActiveStateCommand request,
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

        property.IsActive = request.IsActive;
        property.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
