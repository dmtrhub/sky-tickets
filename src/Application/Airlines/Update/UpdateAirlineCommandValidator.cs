using FluentValidation;

namespace Application.Airlines.Update;

public sealed class UpdateAirlineCommandValidator : AbstractValidator<UpdateAirlineCommand>
{
    public UpdateAirlineCommandValidator()
    {
        RuleFor(c => c.Name)
            .MaximumLength(20)
            .When(c => !string.IsNullOrWhiteSpace(c.Name)); // Validation only if it's passed

        RuleFor(c => c.Address)
            .MaximumLength(50)
            .When(c => !string.IsNullOrWhiteSpace(c.Address));

        RuleFor(c => c.ContactInfo)
            .MaximumLength(50)
            .When(c => !string.IsNullOrWhiteSpace(c.ContactInfo));
    }
}
