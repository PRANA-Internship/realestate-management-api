using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Enums;

namespace RS.Application.Features.Payments.Commands.VerifyPayment;

public class VerifyPaymentCommandHandler(
    IPaymentService paymentService,
    IPaymentTransactionRepository paymentRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<VerifyPaymentCommand, bool>
{
    public async Task<bool> Handle(VerifyPaymentCommand request, CancellationToken ct)
    {
        var transaction = await paymentRepository.GetByTxRefAsync(request.TxRef, ct);
        if (transaction is null)
            return false;

        // Idempotency guard — don't re-process an already-settled transaction
        if (transaction.Status != PaymentStatus.Pending)
            return transaction.Status == PaymentStatus.Success;

        var isVerified = await paymentService.VerifyPaymentAsync(request.TxRef, ct);

        transaction.Status = isVerified ? PaymentStatus.Success : PaymentStatus.Failed;
        await unitOfWork.SaveChangesAsync(ct);

        if (isVerified)
        {
            // TODO: trigger your actual business action here, e.g.:
            // if (transaction.RelatedEntityType == "PropertyBooking")
            //     await bookingService.ConfirmBookingAsync(transaction.RelatedEntityId!.Value, ct);
        }

        return isVerified;
    }
}
