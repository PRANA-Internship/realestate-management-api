using Microsoft.AspNetCore.Authorization;
using RS.Domain.Enums;

namespace RS.Infrastructure.Authentication
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission) : base(policy: permission.ToString())
        {
        }
    }
}
