using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Reviews;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reviews.GetAll;

public sealed class GetAllReviewsQueryHandler(IRepository<Review> reviewRepository)
    : IQueryHandler<GetAllReviewsQuery, List<ReviewResponse>>
{
    public async Task<Result<List<ReviewResponse>>> Handle(GetAllReviewsQuery query, CancellationToken cancellationToken)
    {
        var reviewQuery = await reviewRepository.AsQueryable();

        var reviews = await reviewQuery
            .Include(r => r.User)
            .Include(r => r.Airline)
            .Select(r => r.ToReviewResponse())
            .ToListAsync(cancellationToken);

        if (reviews.Count == 0)
            return Result.Failure<List<ReviewResponse>>(ReviewErrors.NoReviewsFound);

        return reviews;
    }
}
