using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Security.Claims;

namespace Application.Users.GetMyProfile;

public sealed class GetMyProfileQueryHandler(
    IApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor)
    : IQueryHandler<GetMyProfileQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetMyProfileQuery query, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            return Result.Failure<UserResponse>(UserErrors.Unauthenticated);

        var user = await context.Users
            .Where(u => u.Id == parsedUserId)
            .Include(u => u.Reviews)
            .Include(u => u.Reservations)
            .ThenInclude(r => r.Flight)
            .ThenInclude(f => f.Airline)
            .Select(u => u.ToUserResponse())
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return Result.Failure<UserResponse>(UserErrors.NotFound(parsedUserId));

        return user;
    }
}
