using RS.Domain.Enums;

namespace RS.Domain.Entities
{
    public class RolePermission
    {
        public UserRole Role { get; set; }
        public string PermissionName { get; set; } = string.Empty;
    }
}
