using Domain;
using FluentValidation;
using System.Globalization;

namespace Application.Users.Register;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{

    private const string DateFormat = "yyyy-MM-dd";
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(c => c.Password)
            .NotEmpty()
            .MinimumLength(8);

        RuleFor(c => c.DateOfBirth)
            .NotEmpty()
            .Must(BeValidDate).WithMessage($"Date of birth must be in format {DateFormat}.");

        RuleFor(c => c.Gender)
            .Must(BeValidGender).WithMessage("Gender must be either Male or Female.");

    }

    private bool BeValidDate(string date) =>
        DateOnly.TryParseExact(date, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);

    private bool BeValidGender(string gender) =>
        Enum.TryParse<Gender>(gender, true, out _);
}
