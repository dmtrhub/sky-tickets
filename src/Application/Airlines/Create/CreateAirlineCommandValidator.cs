using FluentValidation;

namespace Application.Airlines.Create;

public sealed class CreateAirlineCommandValidator : AbstractValidator<CreateAirlineCommand>
{
    public CreateAirlineCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Airline name is required.")
            .MaximumLength(20).WithMessage("Airline name must not exceed 20 characters.");

        RuleFor(c => c.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(50).WithMessage("Address must not exceed 50 characters.");

        RuleFor(c => c.ContactInfo)
            .NotEmpty().WithMessage("Contact information is required.")
            .MaximumLength(50).WithMessage("Contact information must not exceed 50 characters.");
    }
}
