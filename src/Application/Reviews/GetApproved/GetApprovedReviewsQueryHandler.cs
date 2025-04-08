using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain;
using Domain.Reviews;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reviews.GetApproved;

public sealed class GetApprovedReviewsQueryHandler(IRepository<Review> reviewRepository)
    : IQueryHandler<GetApprovedReviewsQuery, List<ReviewResponse>>
{
    public async Task<Result<List<ReviewResponse>>> Handle(GetApprovedReviewsQuery query, CancellationToken cancellationToken)
    {
        var reviewQuery = await reviewRepository.AsQueryable();

        var reviews = await reviewQuery
            .Where(r => r.Status == ReviewStatus.Approved)
            .Include(r => r.User)
            .Include(r => r.Airline)
            .Select(r => r.ToReviewResponse())
            .ToListAsync(cancellationToken);

        if (reviews.Count == 0)
            return Result.Failure<List<ReviewResponse>>(ReviewErrors.NoReviewsFound);

        return reviews;
    }
}

