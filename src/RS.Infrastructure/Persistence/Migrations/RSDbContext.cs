using Microsoft.EntityFrameworkCore;
using RS.Domain.Entities;

namespace RS.Infrastructure.Persistence.Migrations
{
    using RS.Application.Common.Interfaces;

    public class RSDbContext : DbContext, IUnitOfWork
    {
        public RSDbContext(DbContextOptions<RSDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<Permission> Permissions => Set<Permission>();

        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RSDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
