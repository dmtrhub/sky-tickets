using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Extensions;

public static class UserExtensions
{
    public static Guid? GetUserId(this IHttpContextAccessor httpContextAccessor)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : null;
    }
}
