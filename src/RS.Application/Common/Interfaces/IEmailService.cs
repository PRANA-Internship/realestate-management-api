using System.Threading;
using System.Threading.Tasks;

namespace RS.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendPasswordSetupAsync(string toEmail, string fullName, string token, CancellationToken ct = default);
        Task SendPasswordResetAsync(string toEmail, string fullName, string token, CancellationToken ct = default);
        Task SendWelcomeEmailAsync(string toEmail, string fullName, CancellationToken ct = default);
    }
}
