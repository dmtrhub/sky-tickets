using SharedKernel;

namespace Domain.Reviews;

public static class ReviewErrors
{
    public static Error NotFound(Guid reviewId) =>
        Error.NotFound("Review.NotFound", $"Review with id {reviewId} was not found");
    public static Error AlreadyExists(Guid userId, Guid airlineId) =>
        Error.Conflict("Review.AlreadyExists", $"Review for user {userId} and airline {airlineId} already exists");

    public static readonly Error NotCreated =
        Error.Problem("Review.NotCreated", $"Review was not created");
    public static Error NotApproved(Guid reviewId) =>
        Error.Problem("Review.NotApproved", $"Review with id {reviewId} was not approved");
    public static Error NotRejected(Guid reviewId) =>
        Error.Problem("Review.NotRejected", $"Review with id {reviewId} was not rejected");
    public static Error NoCompletedFlightForAirline(Guid airlineId) =>
        Error.NotFound("Review.NoCompletedFlightForAirline", $"No completed flight found for airline with id {airlineId}");
    public static Error AlreadyReviewed(Guid airlineId) =>
        Error.Conflict("Review.AlreadyReviewed", $"Airline with id {airlineId} is already reviewed");

    public static readonly Error NoReviewsFound =
        Error.NotFound("Review.NoReviewsFound", $"No reviews found in the system");
}
