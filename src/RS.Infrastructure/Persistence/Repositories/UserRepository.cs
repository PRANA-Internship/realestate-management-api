using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RS.Application.Common.Interfaces;
using RS.Domain.Entities;
using RS.Infrastructure.Persistence.Migrations;

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
    }
}
