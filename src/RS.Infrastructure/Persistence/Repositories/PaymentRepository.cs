using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RS.Application.Common.Interfaces;
using RS.Domain.Entities;

namespace RS.Infrastructure.Persistence.Repositories;

public class PaymentRepository(RSDbContext dbContext) : IPaymentRepository
{
    public async Task AddAsync(Payment payment, CancellationToken ct = default)
    {
        await dbContext.Payments.AddAsync(payment, ct);
    }

    public async Task<Payment?> GetByTxRefAsync(string txRef, CancellationToken ct = default)
    {
        return await dbContext.Payments
            .Include(x => x.Reservation)
            .ThenInclude(x => x.Property)
            .FirstOrDefaultAsync(x => x.TxRef == txRef, ct);
    }

    public void Update(Payment payment)
    {
        dbContext.Payments.Update(payment);
    }
}
