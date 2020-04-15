using System;
using System.Security.Claims;
using UserManagementSystem.BLL.Exceptions;

namespace UserManagementSystem.BLL.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetLoggedInUserId(this ClaimsPrincipal principal)
            => Guid.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
                ? id
                : throw new AppUnauthorizedException();
    }
}
