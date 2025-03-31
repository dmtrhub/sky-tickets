using Application.Abstractions.Messaging;

namespace Application.Reviews.Delete;

public sealed record DeleteReviewCommand(Guid Id) : ICommand<Guid>;
