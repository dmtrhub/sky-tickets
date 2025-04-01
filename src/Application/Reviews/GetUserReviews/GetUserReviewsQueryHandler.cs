using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Reviews;
using Domain.Reviews;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Security.Claims;

namespace Application.Reviews.GetUserReviews;

public sealed class GetUserReviewsQueryHandler
    (IApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor
    ) : IQueryHandler<GetUserReviewsQuery, List<ReviewResponse>>
{
    public async Task<Result<List<ReviewResponse>>> Handle(GetUserReviewsQuery query, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            return Result.Failure<List<ReviewResponse>>(UserErrors.Unauthenticated);

        var reviews = await context.Reviews
            .Where(r => r.UserId == parsedUserId)
            .Include(r => r.User)
            .Include(r => r.Airline)
            .Select(r => r.ToReviewResponse())
            .ToListAsync(cancellationToken);

        if (reviews is null)
            return Result.Failure<List<ReviewResponse>>(ReviewErrors.NoReviewsFound);

        return reviews;
    }
}
