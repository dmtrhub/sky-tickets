using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Extensions;
using Domain.Reviews;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reviews.GetUserReviews;

public sealed class GetUserReviewsQueryHandler(
    IRepository<Review> reviewRepository,
    IHttpContextAccessor httpContextAccessor) : IQueryHandler<GetUserReviewsQuery, List<ReviewResponse>>
{
    public async Task<Result<List<ReviewResponse>>> Handle(GetUserReviewsQuery query, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.GetUserId();

        if (userId is null)
            return Result.Failure<List<ReviewResponse>>(UserErrors.Unauthenticated);

        var reviewQuery = await reviewRepository.AsQueryable();

        var reviews = await reviewQuery
            .Where(r => r.UserId == userId)
            .Include(r => r.User)
            .Include(r => r.Airline)
            .Select(r => r.ToReviewResponse())
            .ToListAsync(cancellationToken);

        if (reviews.Count == 0)
            return Result.Failure<List<ReviewResponse>>(ReviewErrors.NoReviewsFound);

        return reviews;
    }
}
