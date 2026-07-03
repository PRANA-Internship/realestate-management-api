// RS.Application/Features/Payments/Commands/InitiatePayment/InitiatePaymentCommandHandler.cs
using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Payments;
using RS.Domain.Entities;
using RS.Domain.Enums;

namespace RS.Application.Features.Payments.Commands.InitiatePayment;

public class InitiatePaymentCommandHandler(
    IPaymentService paymentService,
    IPaymentTransactionRepository paymentRepository,
    IUnitOfWork unitOfWork,
    IUserContext userContext)
    : IRequestHandler<InitiatePaymentCommand, InitializePaymentResponse>
{
    public async Task<InitializePaymentResponse> Handle(InitiatePaymentCommand request, CancellationToken ct)
    {
        var response = await paymentService.InitializePaymentAsync(
            new InitializePaymentRequest(
                request.Amount, request.Currency, request.Email,
                request.FirstName, request.LastName, request.PhoneNumber,
                request.RelatedEntityId, request.RelatedEntityType),
            ct);

        var transaction = new PaymentTransaction
        {
            UserId = userContext.UserId,
            TxRef = response.TxRef,
            Amount = request.Amount,
            Currency = request.Currency,
            Status = PaymentStatus.Pending,
            RelatedEntityId = request.RelatedEntityId,
            RelatedEntityType = request.RelatedEntityType
        };

        await paymentRepository.AddAsync(transaction, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return response;
    }
}
