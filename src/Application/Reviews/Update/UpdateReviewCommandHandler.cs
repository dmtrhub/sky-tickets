using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Reviews;
using SharedKernel;

namespace Application.Reviews.Update;

public sealed class UpdateReviewCommandHandler(
    IRepository<Review> reviewRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateReviewCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateReviewCommand command, CancellationToken cancellationToken)
    {
        var review = await reviewRepository.GetByIdAsync(command.Id, cancellationToken);

        if(review is null)
            return Result.Failure<Guid>(ReviewErrors.NotFound(command.Id));

        review.UpdateReview(command);
        review.Raise(new ReviewUpdatedDomainEvent(review.Id));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return review.Id;
    }
}
