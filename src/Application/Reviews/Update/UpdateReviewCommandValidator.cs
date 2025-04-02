using FluentValidation;

namespace Application.Reviews.Update;

internal sealed class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
    public UpdateReviewCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Title is required and must not exceed 50 characters.")
            .When(c => !string.IsNullOrWhiteSpace(c.Title));

        RuleFor(c => c.Content)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("Content is required and must not exceed 500 characters.")
            .When(c => !string.IsNullOrWhiteSpace(c.Content));
    }
}
