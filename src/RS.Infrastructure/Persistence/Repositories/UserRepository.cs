using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RS.Application.Common.Interfaces;
using RS.Domain.Entities;
using RS.Domain.Enums;

namespace RS.Infrastructure.Persistence.Repositories
{
    public class UserRepository(RSDbContext dbContext) : IUserRepository
    {
        public async Task AddAsync(User user, CancellationToken ct = default)
        {
            await dbContext.Users.AddAsync(user, ct);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
        {
            return await dbContext.Users.AnyAsync(u => u.Email == email, ct);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await dbContext.Users.FindAsync(new object[] { id }, ct);
        }

        public Task UpdateAsync(User user, CancellationToken ct = default)
        {
            dbContext.Users.Update(user);

            return Task.CompletedTask;
        }

        public async Task<User?> GetByResetTokenAsync(string token, CancellationToken ct = default)
        {
            return await dbContext.Users
                .FirstOrDefaultAsync(x => x.PasswordResetToken == token, ct);
        }

        public async Task<IReadOnlyList<User>> GetUsersAsync(
      UserRole? role,
      UserStatus? status,
      string? search,
      CancellationToken ct = default)
        {
            IQueryable<User> query = dbContext.Users;

            if (role.HasValue)
            {
                var roleValue = role.Value;
                query = query.Where(x => x.Role == roleValue);
            }

            if (status.HasValue)
            {
                var statusValue = status.Value;
                query = query.Where(x => x.Status == statusValue);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();

                query = query.Where(x =>
                    x.FullName.ToLower().Contains(search) ||
                    x.Email.ToLower().Contains(search));
            }

            return await query
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<User?> GetByIdWithDetailsAsync(
            Guid id,
            CancellationToken ct = default)
        {
            return await dbContext.Users
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }
    }
}
