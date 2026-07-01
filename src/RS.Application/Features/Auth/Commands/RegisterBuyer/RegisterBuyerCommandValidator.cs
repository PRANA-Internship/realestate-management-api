using FluentValidation;

namespace RS.Application.Features.Auth.Commands.RegisterBuyer
{
    public sealed class RegisterBuyerCommandValidator : AbstractValidator<RegisterBuyerCommand>
    {
        public RegisterBuyerCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is invalid.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        }
    }
}
