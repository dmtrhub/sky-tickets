﻿using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Reviews;
using SharedKernel;

namespace Application.Reviews.Delete;

public sealed class DeleteReviewCommandHandler(
    IRepository<Review> reviewRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteReviewCommand, Guid>
{
    public async Task<Result<Guid>> Handle(DeleteReviewCommand command, CancellationToken cancellationToken)
    {
        var review = await reviewRepository.GetByIdAsync(command.Id, cancellationToken);

        if (review is null)
            return Result.Failure<Guid>(ReviewErrors.NotFound(command.Id));

        review.Raise(new ReviewDeletedDomainEvent(review.Id));

        reviewRepository.Remove(review);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return review.Id;
    }
}
