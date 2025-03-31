using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Reviews;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reviews.GetById;

public sealed class GetReviewByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetReviewByIdQuery, ReviewResponse>
{
    public async Task<Result<ReviewResponse>> Handle(GetReviewByIdQuery query, CancellationToken cancellationToken)
    {
        var review = await context.Reviews
            .Where(r => r.Id == query.Id)
            .Include(r => r.User)
            .Include(r => r.Airline)
            .Select(r => r.ToReviewResponse())
            .FirstOrDefaultAsync(cancellationToken);

        if (review is null)
            return Result.Failure<ReviewResponse>(ReviewErrors.NotFound(query.Id));

        return review;
    }
}