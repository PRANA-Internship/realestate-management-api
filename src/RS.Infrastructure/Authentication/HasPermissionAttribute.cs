using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using RS.Domain.Enums;

namespace RS.Infrastructure.Authentication
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission)
            : base(policy: ToEntityAction(permission.ToString()))
        {
        }

        private static string ToEntityAction(string permissionName) =>
            Regex.Replace(permissionName, "(?<=[a-z])(?=[A-Z])", ":");
    }
}
