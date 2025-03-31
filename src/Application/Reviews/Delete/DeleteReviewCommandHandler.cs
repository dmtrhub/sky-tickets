using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Reviews;
using SharedKernel;

namespace Application.Reviews.Delete;

public sealed class DeleteReviewCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteReviewCommand, Guid>
{
    public async Task<Result<Guid>> Handle(DeleteReviewCommand command, CancellationToken cancellationToken)
    {
        var review = await context.Reviews.FindAsync(command.Id, cancellationToken);

        if (review is null)
            return Result.Failure<Guid>(ReviewErrors.NotFound(command.Id));

        context.Reviews.Remove(review);
        await context.SaveChangesAsync(cancellationToken);

        return review.Id;
    }
}
