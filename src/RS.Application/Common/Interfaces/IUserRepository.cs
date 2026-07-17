using System;
using System.Threading;
using System.Threading.Tasks;
using RS.Domain.Entities;
using RS.Domain.Enums;

namespace RS.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
        Task AddAsync(User user, CancellationToken ct = default);
        Task UpdateAsync(User user, CancellationToken ct = default);

        Task<User?> GetByResetTokenAsync(string token, CancellationToken ct = default);

        Task<PaginatedResult<User>> GetUsersAsync(
        UserRole? role,
        UserStatus? status,
        string? search,
        int page,
        int pageSize,
        CancellationToken ct = default);

        Task<User?> GetByIdWithDetailsAsync(
            Guid id,
            CancellationToken ct = default);

        Task<PaginatedResult<User>> GetSalesByManagerAsync(
     Guid managerId,
     int page,
     int pageSize,
     CancellationToken ct = default);

        Task<User?> GetSalesByManagerAndIdAsync(
            Guid managerId,
            Guid salesId,
            CancellationToken ct = default);

        Task<int> CountAsync(UserRole? role = null,
            CancellationToken ct = default);

    }
}
