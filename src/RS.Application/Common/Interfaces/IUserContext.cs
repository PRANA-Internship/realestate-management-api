using System;
using RS.Domain.Enums;

namespace RS.Application.Common.Interfaces
{
    public interface IUserContext
    {
        Guid UserId { get; }
        UserRole Role { get; }
    }
}
