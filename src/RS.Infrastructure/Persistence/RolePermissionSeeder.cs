using Microsoft.EntityFrameworkCore;
using RS.Domain.Entities;
using RS.Domain.Enums;
using RS.Infrastructure.Persistence;

namespace RS.Infrastructure.Persistence.Configuration
{
    public static class RolePermissionSeeder
    {
        public static async Task SeedAsync(RSDbContext dbContext)
        {
            if (await dbContext.RolePermissions.AnyAsync())
                return; // already seeded, skip

            var rolePermissions = new List<RolePermission>
            {
                // ADMIN - full access
                new() { Role = UserRole.ADMIN, PermissionName = PermissionName.Of(Entity.Property, Permission.List) },
                new() { Role = UserRole.ADMIN, PermissionName = PermissionName.Of(Entity.Property, Permission.Read) },
                new() { Role = UserRole.ADMIN, PermissionName = PermissionName.Of(Entity.Property, Permission.Create) },
                new() { Role = UserRole.ADMIN, PermissionName = PermissionName.Of(Entity.Property, Permission.Update) },
                new() { Role = UserRole.ADMIN, PermissionName = PermissionName.Of(Entity.Property, Permission.Delete) },
                new() { Role = UserRole.ADMIN, PermissionName = PermissionName.Of(Entity.PropertyImage, Permission.Create) },
                new() { Role = UserRole.ADMIN, PermissionName = PermissionName.Of(Entity.PropertyImage, Permission.Update) },
                new() { Role = UserRole.ADMIN, PermissionName = PermissionName.Of(Entity.PropertyImage, Permission.Delete) },
                new() { Role = UserRole.ADMIN, PermissionName = PermissionName.Of(Entity.Configuration, Permission.Update) },

                // MANAGER - operational access, no configuration
                new() { Role = UserRole.MANAGER, PermissionName = PermissionName.Of(Entity.Property, Permission.List) },
                new() { Role = UserRole.MANAGER, PermissionName = PermissionName.Of(Entity.Property, Permission.Read) },
                new() { Role = UserRole.MANAGER, PermissionName = PermissionName.Of(Entity.Property, Permission.Create) },
                new() { Role = UserRole.MANAGER, PermissionName = PermissionName.Of(Entity.Property, Permission.Update) },
                new() { Role = UserRole.MANAGER, PermissionName = PermissionName.Of(Entity.PropertyImage, Permission.Create) },
                new() { Role = UserRole.MANAGER, PermissionName = PermissionName.Of(Entity.PropertyImage, Permission.Update) },

                // SALES - list/read properties
                new() { Role = UserRole.SALES, PermissionName = PermissionName.Of(Entity.Property, Permission.List) },
                new() { Role = UserRole.SALES, PermissionName = PermissionName.Of(Entity.Property, Permission.Read) },

                // BUYER - read-only, browsing properties
                new() { Role = UserRole.BUYER, PermissionName = PermissionName.Of(Entity.Property, Permission.List) },
                new() { Role = UserRole.BUYER, PermissionName = PermissionName.Of(Entity.Property, Permission.Read) },
            };

            await dbContext.RolePermissions.AddRangeAsync(rolePermissions);
            await dbContext.SaveChangesAsync();
        }
    }
}
