using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Reviews;
using SharedKernel;

namespace Application.Reviews.Update;

public sealed class UpdateReviewCommandHandler(IApplicationDbContext context)
    : ICommandHandler<UpdateReviewCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateReviewCommand command, CancellationToken cancellationToken)
    {
        var review = await context.Reviews.FindAsync(command.Id, cancellationToken);

        if(review is null)
            return Result.Failure<Guid>(ReviewErrors.NotFound(command.Id));

        review.UpdateReview(command);

        await context.SaveChangesAsync(cancellationToken);

        return review.Id;
    }
}
