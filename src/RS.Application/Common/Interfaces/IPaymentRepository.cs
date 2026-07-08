using System;
using System.Threading;
using System.Threading.Tasks;
using RS.Domain.Entities;

namespace RS.Application.Common.Interfaces;

public interface IPaymentRepository
{
    Task AddAsync(Payment payment, CancellationToken ct = default);
    Task<Payment?> GetByTxRefAsync(string txRef, CancellationToken ct = default);
    void Update(Payment payment);
}
