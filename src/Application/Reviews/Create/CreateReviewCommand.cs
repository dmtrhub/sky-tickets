using Application.Abstractions.Messaging;

namespace Application.Reviews.Create;

public sealed record CreateReviewCommand(
    Guid AirlineId,
    string Title,
    string Content,
    string? ImageUrl) : ICommand<Guid>;
