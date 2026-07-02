using RS.Domain.Constants;
using RS.Domain.Entities;
using RS.Domain.Enums;
using RS.Infrastructure.Persistence.Migrations;

namespace RS.Infrastructure.Persistence.Seed;

public static class PermissionSeeder
{
    public static async Task SeedAsync(RSDbContext context)
    {
        // Prevent duplicate seeding (safe check)
        if (context.Permissions.Any())
            return;

        var permissions = new List<Permission>
        {
            new() { Name = Permissions.Property.Create },
            new() { Name = Permissions.Property.Read },
            new() { Name = Permissions.Property.Update },
            new() { Name = Permissions.Property.Delete },
            new() { Name = Permissions.Property.List },

            new() { Name = Permissions.User.Create },
            new() { Name = Permissions.User.Read },
            new() { Name = Permissions.User.Update },
            new() { Name = Permissions.User.Delete },
            new() { Name = Permissions.User.List }
        };

        await context.Permissions.AddRangeAsync(permissions);
        await context.SaveChangesAsync();

        var adminPermissions = permissions.Select(p => new RolePermission
        {
            Role = UserRole.ADMIN,   // ✅ FIXED (was ADMIN)
            PermissionId = p.Id
        });

        await context.RolePermissions.AddRangeAsync(adminPermissions);
        await context.SaveChangesAsync();
    }
}
