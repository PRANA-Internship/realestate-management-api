using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RS.Application.Common.Interfaces;
using RS.Domain.Entities;

namespace RS.Infrastructure.Persistence
{
    public class RSDbContext : DbContext, IUnitOfWork
    {
        public RSDbContext(DbContextOptions<RSDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }

        public DbSet<SystemConfiguration> SystemConfigurations { get; set; }
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
        }
    }
}
