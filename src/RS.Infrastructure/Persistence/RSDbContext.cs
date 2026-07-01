using Microsoft.EntityFrameworkCore;
using RS.Application.Common.Interfaces;
using RS.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace RS.Infrastructure.Persistence
{
    public class RSDbContext : DbContext, IUnitOfWork
    {
        public RSDbContext(DbContextOptions<RSDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RSDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
