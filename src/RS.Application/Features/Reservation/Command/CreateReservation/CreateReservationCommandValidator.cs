using FluentValidation;

namespace RS.Application.Features.Reservations.Commands.CreateReservation;

public class CreateReservationCommandValidator
    : AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty();

        RuleFor(x => x.BuyerFullName)
            .MaximumLength(150);

        RuleFor(x => x.BuyerEmail)
            .MaximumLength(200);

        RuleFor(x => x.BuyerPhoneNumber)
            .MaximumLength(30);

        When(x => !string.IsNullOrWhiteSpace(x.BuyerEmail), () =>
        {
            RuleFor(x => x.BuyerEmail!)
                .EmailAddress();
        });
    }
}
