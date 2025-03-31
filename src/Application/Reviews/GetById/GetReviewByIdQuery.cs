using Application.Abstractions.Messaging;

namespace Application.Reviews.GetById;

public sealed record GetReviewByIdQuery(Guid Id) : IQuery<ReviewResponse>;
