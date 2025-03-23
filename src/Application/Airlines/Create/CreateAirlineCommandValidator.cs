using FluentValidation;

namespace Application.Airlines.Create;

internal sealed class CreateAirlineCommandValidator : AbstractValidator<CreateAirlineCommand>
{
    public CreateAirlineCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(c => c.Address)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.ContactInfo)
            .NotEmpty()
            .MaximumLength(50);
    }
}
