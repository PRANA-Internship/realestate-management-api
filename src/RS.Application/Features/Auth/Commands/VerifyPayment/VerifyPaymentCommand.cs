using MediatR;

namespace RS.Application.Features.Payments.Commands.VerifyPayment;

public record VerifyPaymentCommand(string TxRef) : IRequest<bool>;
