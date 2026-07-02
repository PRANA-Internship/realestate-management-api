using Microsoft.EntityFrameworkCore;
using RS.Application.Common.Interfaces;
using RS.Domain.Entities;
using RS.Infrastructure.Persistence.Migrations;

namespace RS.Infrastructure.Services;

public class PermissionService : IPermissionService
{
    private readonly RSDbContext _context;

    public PermissionService(RSDbContext context)
    {
        _context = context;
    }

    public async Task<bool> HasPermissionAsync(Guid userId, string permission)
    {
        var userRole = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => u.Role)
            .FirstOrDefaultAsync();

        if (userRole == null)
            return false;

        return await _context.RolePermissions
            .Include(rp => rp.Permission)
            .AnyAsync(rp =>
                rp.Role == userRole &&
                rp.Permission.Name == permission);
    }
}
