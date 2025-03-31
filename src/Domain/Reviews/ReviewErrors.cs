using SharedKernel;

namespace Domain.Reviews;

public static class ReviewErrors
{
    public static Error NotFound(Guid reviewId) =>
        Error.NotFound("Review.NotFound", $"Review with id {reviewId} was not found");
    public static Error AlreadyExists(Guid userId, Guid airlineId) =>
        Error.Conflict("Review.AlreadyExists", $"Review for user {userId} and airline {airlineId} already exists");
    public static Error NotCreated(Guid userId, Guid airlineId) =>
        Error.Problem("Review.NotCreated", $"Review for user {userId} and airline {airlineId} was not created");
    public static Error NotApproved(Guid reviewId) =>
        Error.Problem("Review.NotApproved", $"Review with id {reviewId} was not approved");
    public static Error NotRejected(Guid reviewId) =>
        Error.Problem("Review.NotRejected", $"Review with id {reviewId} was not rejected");

    public static readonly Error NoReviewsFound =
        Error.NotFound("Review.NoReviewsFound", $"No reviews found in the system");
}
