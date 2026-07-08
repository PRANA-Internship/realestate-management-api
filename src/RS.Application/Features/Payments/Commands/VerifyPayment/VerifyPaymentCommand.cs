using MediatR;
using RS.Domain.Common;

namespace RS.Application.Features.Payments.Commands.VerifyPayment;

public record VerifyPaymentCommand(string TxRef) : IRequest<Result<bool>>;
