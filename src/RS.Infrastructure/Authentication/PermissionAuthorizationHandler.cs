using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RS.Domain.Enums;

namespace RS.Infrastructure.Authentication
{
    public class PermissionAuthorizationHandler(IPermissionProvider permissionProvider)
        : AuthorizationHandler<PermissionRequirement>
    {
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var roleClaim = context.User.FindFirst(ClaimTypes.Role)?.Value;

            if ((Enum.TryParse<UserRole>(roleClaim, true, out var role) && (await permissionProvider.HasPermissionAsync(role, requirement.Permission))))
            {
                context.Succeed(requirement);
            }
        }
    }

}
