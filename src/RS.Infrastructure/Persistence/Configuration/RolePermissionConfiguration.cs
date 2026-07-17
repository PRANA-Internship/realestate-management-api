using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RS.Domain.Entities;

namespace RS.Infrastructure.Persistence.Configuration
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(rp => new { rp.Role, rp.PermissionName });

            builder.Property(rp => rp.Role)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(rp => rp.PermissionName)
                .HasMaxLength(100);
        }
    }
}
