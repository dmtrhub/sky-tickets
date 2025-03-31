using Application.Abstractions.Messaging;

namespace Application.Reviews.Update;

public sealed record UpdateReviewCommand(
    Guid Id,
    string Title,
    string Content,
    string? ImageUrl) : ICommand<Guid>;
