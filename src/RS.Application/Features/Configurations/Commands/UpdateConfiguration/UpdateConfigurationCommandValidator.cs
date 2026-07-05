using FluentValidation;

namespace RS.Application.Features.Configurations.Commands.UpdateConfiguration;

public class UpdateConfigurationCommandValidator
    : AbstractValidator<UpdateConfigurationCommand>
{
    public UpdateConfigurationCommandValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Value)
            .NotEmpty()
            .MaximumLength(500);
    }
}
