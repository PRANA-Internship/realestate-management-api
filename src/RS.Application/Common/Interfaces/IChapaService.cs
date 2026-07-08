using System.Threading;
using System.Threading.Tasks;
using RS.Application.Common.Models;

namespace RS.Application.Common.Interfaces;

public interface IChapaService
{
    Task<ChapaInitializeResponse> InitializePaymentAsync(ChapaInitializeRequest request, CancellationToken ct = default);
    Task<ChapaVerifyResponse> VerifyTransactionAsync(string txRef, CancellationToken ct = default);
}
