using Domain;
using FluentValidation;
using System.Globalization;

namespace Application.Users.Update;

internal sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{

    private const string DateFormat = "yyyy-MM-dd";
    public UpdateUserCommandValidator()
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

        RuleFor(c => c.Role)
            .Must(BeValidRole).WithMessage("Role must be either Passenger or Administrator.");
    }

    private bool BeValidDate(string date) =>
        DateOnly.TryParseExact(date, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);

    private bool BeValidGender(string gender) =>
        Enum.TryParse<Gender>(gender, true, out _);

    private bool BeValidRole(string role) =>
        Enum.TryParse<UserRole>(role, true, out _);
}
