using Application.Abstractions.Messaging;

namespace Application.Reviews.Approve;

public sealed record ApproveReviewCommand(Guid Id, bool IsApproved) : ICommand<Guid>;

