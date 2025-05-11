using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace Application.UnitTests.Extensions;

public static class HttpContextAccessorExtensionsForTests
{
    public static void SetUserId(this Mock<IHttpContextAccessor> httpContextAccessorMock, Guid? userId)
    {
        var claims = new List<Claim>();
        if (userId.HasValue)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.ToString()!));
        }

        var identity = new ClaimsIdentity(claims);
        var user = new ClaimsPrincipal(identity);

        httpContextAccessorMock.Setup(x => x.HttpContext!.User).Returns(user);
    }
}
