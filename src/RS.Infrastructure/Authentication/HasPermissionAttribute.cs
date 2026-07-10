using Microsoft.AspNetCore.Authorization;
using RS.Domain.Enums;

namespace RS.Infrastructure.Authentication
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Entity entity, Permission permission)
            : base(policy: PermissionName.Of(entity, permission))
        {
        }
    }
}
