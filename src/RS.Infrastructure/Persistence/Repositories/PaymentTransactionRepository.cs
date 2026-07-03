using Microsoft.EntityFrameworkCore;
using RS.Application.Common.Interfaces;
using RS.Domain.Entities;
using RS.Infrastructure.Persistence.Migrations;

namespace RS.Infrastructure.Persistence.Repositories;

public class PaymentTransactionRepository(RSDbContext context) : IPaymentTransactionRepository
{
    public async Task AddAsync(PaymentTransaction transaction, CancellationToken ct = default)
    {
        await context.PaymentTransactions.AddAsync(transaction, ct);
    }

    public async Task<PaymentTransaction?> GetByTxRefAsync(string txRef, CancellationToken ct = default)
    {
        return await context.PaymentTransactions
            .FirstOrDefaultAsync(t => t.TxRef == txRef, ct);
    }
}
