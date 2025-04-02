using FluentValidation;

namespace Application.Reviews.Approve;

internal sealed class ApproveReviewCommandValidator : AbstractValidator<ApproveReviewCommand>
{
    public ApproveReviewCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("Review ID is required.");

        RuleFor(c => c.IsApproved)
            .NotNull()
            .WithMessage("IsApproved is required.");
    }
}
