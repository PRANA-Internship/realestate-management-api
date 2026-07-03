using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RS.Application.Common.Interfaces;
using RS.Domain.Entities;

namespace RS.Infrastructure.Persistence
namespace RS.Infrastructure.Persistence.Migrations
{
    public class RSDbContext : DbContext, IUnitOfWork
    {
        public RSDbContext(DbContextOptions<RSDbContext> options) : base(options) { }
        public RSDbContext(DbContextOptions<RSDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<Permission> Permissions => Set<Permission>();

        public DbSet<User> Users { get; set; }
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RSDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
            base.OnModelCreating(modelBuilder);
        }
    }
}
