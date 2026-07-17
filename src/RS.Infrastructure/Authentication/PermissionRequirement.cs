using Microsoft.AspNetCore.Authorization;

namespace RS.Infrastructure.Authentication
{
    public class PermissionRequirement(string permission) : IAuthorizationRequirement
    {
        public string Permission { get; } = permission;
    }
}
