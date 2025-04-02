using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain;
using Domain.Reviews;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reviews.GetApproved;

public sealed class GetApprovedReviewsQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetApprovedReviewsQuery, List<ReviewResponse>>
{
    public async Task<Result<List<ReviewResponse>>> Handle(GetApprovedReviewsQuery query, CancellationToken cancellationToken)
    {
        var reviews = await context.Reviews
            .Where(r => r.Status == ReviewStatus.Approved)
            .Include(r => r.User)
            .Include(r => r.Airline)
            .Select(r => r.ToReviewResponse())
            .ToListAsync(cancellationToken);

        if (reviews is null)
            return Result.Failure<List<ReviewResponse>>(ReviewErrors.NoReviewsFound);

        return reviews;
    }
}

