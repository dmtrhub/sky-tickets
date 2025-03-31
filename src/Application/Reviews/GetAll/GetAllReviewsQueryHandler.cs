using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Reviews;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reviews.GetAll;

public sealed class GetAllReviewsQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetAllReviewsQuery, List<ReviewResponse>>
{
    public async Task<Result<List<ReviewResponse>>> Handle(GetAllReviewsQuery query, CancellationToken cancellationToken)
    {
        var reviews = await context.Reviews
            .Include(r => r.User)
            .Include(r => r.Airline)
            .Select(r => r.ToReviewResponse())
            .ToListAsync(cancellationToken);

        if(reviews is null)
            return Result.Failure<List<ReviewResponse>>(ReviewErrors.NoReviewsFound);

        return reviews;
    }
}
