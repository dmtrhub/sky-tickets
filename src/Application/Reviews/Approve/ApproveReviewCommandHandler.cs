using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Reviews;
using Domain;
using SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Application.Reviews.Approve;

public sealed class ApproveReviewCommandHandler(IApplicationDbContext context)
    : ICommandHandler<ApproveReviewCommand, Guid>
{
    public async Task<Result<Guid>> Handle(ApproveReviewCommand command, CancellationToken cancellationToken)
    {
        var review = await context.Reviews
            .FirstOrDefaultAsync(r => r.Id == command.Id, cancellationToken);

        if (review is null)
            return Result.Failure<Guid>(ReviewErrors.NotFound(command.Id));

        if (review.Status != ReviewStatus.Created)
            return Result.Failure<Guid>(ReviewErrors.NotCreated);

        if(command.IsApproved)
        {
            review.Status = ReviewStatus.Approved;
            review.Raise(new ReviewApprovedDomainEvent(review.Id));
        }
        else
        {
            review.Status = ReviewStatus.Rejected;
            review.Raise(new ReviewRejectedDomainEvent(review.Id));
        }

        await context.SaveChangesAsync(cancellationToken);

        return review.Id;
    }
}

