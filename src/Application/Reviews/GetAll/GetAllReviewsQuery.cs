using Application.Abstractions.Messaging;

namespace Application.Reviews.GetAll;

public sealed record GetAllReviewsQuery() : IQuery<List<ReviewResponse>>;
