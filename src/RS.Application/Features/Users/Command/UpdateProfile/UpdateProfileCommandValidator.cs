using FluentValidation;

namespace RS.Application.Features.Users.Commands.UpdateProfile;

public class UpdateProfileCommandValidator
    : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.FullName)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.FullName));

        RuleFor(x => x.Phone)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x)
            .Must(x =>
                !string.IsNullOrWhiteSpace(x.FullName) ||
                !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage("At least one field must be provided.");
    }
}
