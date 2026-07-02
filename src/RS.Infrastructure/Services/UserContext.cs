using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using RS.Application.Common.Interfaces;
using RS.Domain.Enums;

namespace RS.Infrastructure.Services
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return claim != null && Guid.TryParse(claim, out Guid id) ? id : Guid.Empty;
            }
        }

        public UserRole Role
        {
            get
            {
                var roleClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
                return Enum.TryParse<UserRole>(roleClaim, true, out var role) ? role : UserRole.BUYER;
            }
        }
    }
}
