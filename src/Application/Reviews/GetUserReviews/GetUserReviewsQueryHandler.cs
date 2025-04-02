using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Extensions;
using Domain.Reviews;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reviews.GetUserReviews;

public sealed class GetUserReviewsQueryHandler(
    IApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor) : IQueryHandler<GetUserReviewsQuery, List<ReviewResponse>>
{
    public async Task<Result<List<ReviewResponse>>> Handle(GetUserReviewsQuery query, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.GetUserId();

        if (userId is null)
            return Result.Failure<List<ReviewResponse>>(UserErrors.Unauthenticated);

        var reviews = await context.Reviews
            .Where(r => r.UserId == userId)
            .Include(r => r.User)
            .Include(r => r.Airline)
            .Select(r => r.ToReviewResponse())
            .ToListAsync(cancellationToken);

        if (reviews is null)
            return Result.Failure<List<ReviewResponse>>(ReviewErrors.NoReviewsFound);

        return reviews;
    }
}
