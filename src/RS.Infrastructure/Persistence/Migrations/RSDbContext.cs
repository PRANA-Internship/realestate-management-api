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

<<<<<<< HEAD:src/RS.Infrastructure/Persistence/Migrations/RSDbContext.cs
        public DbSet<User> Users => Set<User>();

        public DbSet<Permission> Permissions => Set<Permission>();

        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RSDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
=======
        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RSDbContext).Assembly);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Property>()
    .Property(p => p.Type)
    .HasConversion<string>();

            modelBuilder.Entity<Property>()
                .Property(p => p.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Property>()
    .HasMany(p => p.Images)
    .WithOne(i => i.Property)
    .HasForeignKey(i => i.PropertyId)
    .OnDelete(DeleteBehavior.Cascade);

        }



        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
>>>>>>> origin/main:src/RS.Infrastructure/Persistence/RSDbContext.cs
        }
    }
}
