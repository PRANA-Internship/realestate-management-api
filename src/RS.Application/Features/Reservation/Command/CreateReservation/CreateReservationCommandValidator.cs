using FluentValidation;
using RS.Application.Common.Interfaces;

namespace RS.Application.Features.Reservations.Commands.CreateReservation;

public class CreateReservationCommandValidator
    : AbstractValidator<CreateReservationCommand>
{
    private readonly IUserContext _userContext;

    public CreateReservationCommandValidator(IUserContext userContext)
    {
        _userContext = userContext;

        RuleFor(x => x.PropertyId)
            .NotEmpty();


        When(x => _userContext.UserId == Guid.Empty, () =>
        {
            RuleFor(x => x.BuyerFullName)
                .NotEmpty().WithMessage("Full name is required for guest users.")
                .MaximumLength(150);

            RuleFor(x => x.BuyerEmail)
                .NotEmpty().WithMessage("Email is required for guest users.")
                .EmailAddress()
                .MaximumLength(200);

            RuleFor(x => x.BuyerPhoneNumber)
                .NotEmpty().WithMessage("Phone number is required for guest users.")
                .MaximumLength(30);
        });


        When(x => !string.IsNullOrWhiteSpace(x.BuyerEmail), () =>
        {
            RuleFor(x => x.BuyerEmail!)
                .EmailAddress();
        });
    }
}
