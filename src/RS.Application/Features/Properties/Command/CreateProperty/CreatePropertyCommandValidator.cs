using FluentValidation;
using Microsoft.AspNetCore.Http;
using RS.Application.Features.Properties.Command.CreateProperty;
using RS.Domain.Enums;

namespace RS.Application.Features.Properties.Commands.CreateProperty;

public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
{
    private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

    private static readonly string[] AllowedExtensions =
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

    public CreatePropertyCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty();

        RuleFor(x => x.Location)
            .NotEmpty()
            .MaximumLength(300);

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.AreaSize)
            .GreaterThan(0);

        RuleFor(x => x.Bedrooms)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Bathrooms)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Type)
            .Must(type => Enum.TryParse<PropertyType>(type, true, out _))
            .WithMessage("Invalid property type.");

        RuleFor(x => x.Status)
            .Must(status => Enum.TryParse<PropertyStatus>(status, true, out _))
            .WithMessage("Invalid property status.");

        RuleFor(x => x.Images)
            .NotNull()
            .WithMessage("At least one property image is required.")
            .Must(images => images.Count > 0)
            .WithMessage("At least one property image is required.");

        RuleForEach(x => x.Images)
            .NotNull()
            .Must(BeValidSize)
            .WithMessage($"Each image must not exceed {MaxFileSize / (1024 * 1024)} MB.")
            .Must(BeValidExtension)
            .WithMessage("Only JPG, JPEG, PNG and WEBP images are allowed.");
    }

    private static bool BeValidSize(IFormFile file)
    {
        return file.Length > 0 && file.Length <= MaxFileSize;
    }

    private static bool BeValidExtension(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);

        return AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
    }
}
