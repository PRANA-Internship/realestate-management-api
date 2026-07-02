using Microsoft.AspNetCore.Authorization;

namespace RS.Application.Security.Permissions;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public const string PolicyPrefix = "PERMISSION:";

    public HasPermissionAttribute(string permission)
    {
        Policy = PolicyPrefix + permission;
    }
}
