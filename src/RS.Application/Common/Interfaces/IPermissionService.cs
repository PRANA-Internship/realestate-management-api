using System;
using System.Collections.Generic;
using System.Text;

namespace RS.Application.Common.Interfaces
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(Guid userId, string permission);
    }
}
