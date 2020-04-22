using System;
using System.Security.Claims;
using UserManagementSystem.BLL.Exceptions;
using UserManagementSystem.DAL.Enums;

namespace UserManagementSystem.BLL.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetCurrentUserId(this ClaimsPrincipal principal)
            => Guid.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
                ? id
                : throw new AppUnauthorizedException();

        public static RoleType GetCurrentUserRole(this ClaimsPrincipal principal)
            => Enum.TryParse<RoleType>(principal.FindFirstValue(ClaimTypes.Role), out var role)
                ? role
                : throw new AppUnauthorizedException();
    }
}
