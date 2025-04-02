using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Extensions;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.GetMyProfile;

public sealed class GetMyProfileQueryHandler(
    IApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor) : IQueryHandler<GetMyProfileQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetMyProfileQuery query, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.GetUserId();

        if (userId is null)
            return Result.Failure<UserResponse>(UserErrors.Unauthenticated);

        var user = await context.Users
            .Where(u => u.Id == userId)
            .Include(u => u.Reviews)
            .Include(u => u.Reservations)
            .ThenInclude(r => r.Flight)
            .ThenInclude(f => f.Airline)
            .Select(u => u.ToUserResponse())
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return Result.Failure<UserResponse>(UserErrors.NotFound(userId.Value));

        return user;
    }
}
