using Application.Abstractions.Messaging;

namespace Application.Reviews.GetApproved;

public sealed record GetApprovedReviewsQuery : IQuery<List<ReviewResponse>>;

