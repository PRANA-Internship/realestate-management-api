using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RS.Domain.Entities;
using RS.Domain.Enums;
using RS.Infrastructure.Persistence;

namespace RS.Infrastructure.Authentication
{
    public class DbPermissionProvider(RSDbContext dbContext) : IPermissionProvider
    {
        public async Task<bool> HasPermissionAsync(UserRole role, string permissionName)
        {
            return await dbContext.RolePermissions
                .AnyAsync(rp => rp.Role == role && rp.PermissionName == permissionName);
        }
    }
}
