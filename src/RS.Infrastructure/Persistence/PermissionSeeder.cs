using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RS.Domain.Entities;
using RS.Domain.Enums;

namespace RS.Infrastructure.Persistence
{
    public static class PermissionSeeder
    {
        public static async Task SeedAsync(RSDbContext context)
        {
            if (context.RolePermissions.Any())
                return;

            var rolePermissions = new List<RolePermission>();

            // ADMIN gets every permission
            foreach (Permission permission in Enum.GetValues<Permission>())
            {
                rolePermissions.Add(new RolePermission
                {
                    Role = UserRole.ADMIN,
                    PermissionName = permission.ToString()
                });
            }

            Permission[] managerPermissions =
  {
    Permission.PropertyCreate,
    Permission.PropertyReadMy,
    Permission.PropertyRead,
    Permission.PropertyUpdate,
    Permission.PropertyDelete,
    Permission.PropertyImageAdd,
    Permission.PropertyImageDelete,
    Permission.PropertyImageSetPrimary,
    Permission.PropertyActiveStateChange
};

            foreach (var permission in managerPermissions)
            {
                rolePermissions.Add(new RolePermission
                {
                    Role = UserRole.MANAGER,
                    PermissionName = permission.ToString()
                });
            }

            await context.RolePermissions.AddRangeAsync(rolePermissions);
            await context.SaveChangesAsync();
        }
    }
}
