using Application.Abstractions.Messaging;

namespace Application.Reviews.GetUserReviews;

public sealed record GetUserReviewsQuery() : IQuery<List<ReviewResponse>>;
