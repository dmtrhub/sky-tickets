using FluentValidation;

namespace Application.Reviews.Create;

public sealed class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(c => c.AirlineId)
            .NotEmpty()
            .WithMessage("Airline ID is required.");

        RuleFor(c => c.Title)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Title is required and must not exceed 50 characters.");

        RuleFor(c => c.Content)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("Content is required and must not exceed 500 characters.");
    }
}
