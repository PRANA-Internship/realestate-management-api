using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace RS.Application.Features.Properties.Commands.AddPropertyImages;

public class AddPropertyImagesCommandValidator
    : AbstractValidator<AddPropertyImagesCommand>
{
    private const long MaxFileSize = 5 * 1024 * 1024;

    private static readonly string[] AllowedExtensions =
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

    public AddPropertyImagesCommandValidator()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty();

        RuleFor(x => x.Images)
            .NotNull()
            .Must(images => images.Count > 0)
            .WithMessage("At least one image is required.");

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

        return AllowedExtensions.Contains(
            extension,
            StringComparer.OrdinalIgnoreCase);
    }
}
