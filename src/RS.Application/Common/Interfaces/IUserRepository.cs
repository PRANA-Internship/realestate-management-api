using System;
using System.Threading;
using System.Threading.Tasks;
using RS.Domain.Entities;

namespace RS.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
        Task AddAsync(User user, CancellationToken ct = default);
        Task UpdateAsync(User user, CancellationToken ct = default);
    }
}
