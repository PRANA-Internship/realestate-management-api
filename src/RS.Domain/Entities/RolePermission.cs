using RS.Domain.Common;
using RS.Domain.Enums;

namespace RS.Domain.Entities;

public class RolePermission : BaseEntity
{
    public UserRole Role { get; set; }

    public Guid PermissionId { get; set; }

    public Permission Permission { get; set; } = null!;
}
