using MediatR;
using RS.Contracts.Payments;

namespace RS.Application.Features.Payments.Commands.InitiatePayment;

public record InitiatePaymentCommand(
    decimal Amount,
    string Currency,
    string Email,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    Guid? RelatedEntityId,
    string? RelatedEntityType
) : IRequest<InitializePaymentResponse>;
