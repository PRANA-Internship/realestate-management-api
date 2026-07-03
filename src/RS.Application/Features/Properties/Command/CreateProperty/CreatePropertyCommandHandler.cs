using System;
using MediatR;
using RS.Application.Common.Interfaces;
using RS.Application.Features.Properties.Command.CreateProperty;
using RS.Domain.Common;
using RS.Domain.Entities;
using RS.Domain.Enums;

namespace RS.Application.Features.Properties.Commands.CreateProperty;

public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IUserContext _userContext;
    private readonly IStorageService _storageService;

    public CreatePropertyCommandHandler(
        IUnitOfWork unitOfWork,
        IPropertyRepository propertyRepository,
        IUserContext userContext,
        IStorageService storageService)
    {
        _unitOfWork = unitOfWork;
        _propertyRepository = propertyRepository;
        _userContext = userContext;
        _storageService = storageService;
    }

    public async Task<Result<Guid>> Handle(CreatePropertyCommand request, CancellationToken ct)
    {
        if (_userContext.UserId == Guid.Empty)
        {
            return Result<Guid>.Failure(
                new Error("AUTH_001", "User is not authenticated."));
        }

        var type = Enum.Parse<PropertyType>(request.Type, true);
        var status = Enum.Parse<PropertyStatus>(request.Status, true);

        var property = new Property
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Type = type,
            Status = status,
            Price = request.Price,
            Location = request.Location,
            City = request.City,
            Country = request.Country,
            Bedrooms = request.Bedrooms,
            Bathrooms = request.Bathrooms,
            AreaSize = request.AreaSize,

            CreatedByUserId = _userContext.UserId,
            CreatedByRole = _userContext.Role.ToString(),

            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var uploadedImageUrls = new List<string>();

        try
        {
            foreach (var image in request.Images)
            {
                var imageUrl = await _storageService.UploadImageAsync(image, ct);

                uploadedImageUrls.Add(imageUrl);

                property.Images.Add(new PropertyImage
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = imageUrl,
                    IsPrimary = property.Images.Count == 0,
                    DisplayOrder = property.Images.Count + 1,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _propertyRepository.AddAsync(property, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result<Guid>.Success(property.Id);
        }
        catch
        {
            foreach (var imageUrl in uploadedImageUrls)
            {
                try
                {
                    await _storageService.DeleteImageAsync(imageUrl, ct);
                }
                catch (Exception)
                {
                    // Ignore cleanup errors.
                }
            }

            throw;
        }
    }
}
