using Application.Abstractions.Messaging;

namespace Application.Reviews.GetAll;

public sealed class GetAllReviewsQuery() : IQuery<List<ReviewResponse>>;
