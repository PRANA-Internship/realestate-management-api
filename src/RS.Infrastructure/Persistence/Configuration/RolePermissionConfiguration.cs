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

            // Seed initial data
            var data = new List<RolePermission>();

            // Admin role permissions (All permissions)
            foreach (Permission p in Enum.GetValues<Permission>())
            {
                data.Add(new RolePermission
                {
                    Role = UserRole.ADMIN,
                    PermissionName = p.ToString()
                });
            }

            // Manager role permissions
            var managerPermissions = new[]
            {
                Permission.CreateProperty,
                Permission.ReadMyProperties,
                Permission.ReadProperty,
                Permission.UpdateProperty,
                Permission.DeleteProperty,
                Permission.AddPropertyImages,
                Permission.DeletePropertyImage,
                Permission.SetPrimaryPropertyImage,
                Permission.ChangePropertyActiveState
            };

            foreach (var p in managerPermissions)
            {
                data.Add(new RolePermission
                {
                    Role = UserRole.MANAGER,
                    PermissionName = p.ToString()
                });
            }

            builder.HasData(data);
        }
    }
}
