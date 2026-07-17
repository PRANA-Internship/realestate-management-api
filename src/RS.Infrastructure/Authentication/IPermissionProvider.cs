using System.Threading.Tasks;
using RS.Domain.Enums;

namespace RS.Infrastructure.Authentication
{
    public interface IPermissionProvider
    {
        Task<bool> HasPermissionAsync(UserRole role, string permissionName);
    }
}
