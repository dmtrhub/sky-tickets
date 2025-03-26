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
            .MaximumLength(50)
            .When(c => !string.IsNullOrWhiteSpace(c.FirstName));

        RuleFor(c => c.LastName)
            .NotEmpty()
            .MaximumLength(50)
            .When(c => !string.IsNullOrWhiteSpace(c.LastName));

        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress()
            .When(c => !string.IsNullOrWhiteSpace(c.Email));

        RuleFor(c => c.Password)
            .NotEmpty()
            .MinimumLength(8)
            .When(c => !string.IsNullOrWhiteSpace(c.Password));

        RuleFor(c => c.DateOfBirth)
            .NotEmpty()
            .Must(BeValidDate).WithMessage($"Date of birth must be in format {DateFormat}.")
            .When(c => !string.IsNullOrWhiteSpace(c.DateOfBirth));

        RuleFor(c => c.Gender)
            .Must(BeValidGender).WithMessage("Gender must be either Male or Female.")
            .When(c => !string.IsNullOrWhiteSpace(c.Gender));

        RuleFor(c => c.Role)
            .Must(BeValidRole).WithMessage("Role must be either Passenger or Administrator.")
            .When(c => !string.IsNullOrWhiteSpace(c.Role));
    }

    private bool BeValidDate(string? date) =>
        DateOnly.TryParseExact(date, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);

    private bool BeValidGender(string? gender) =>
        Enum.TryParse<Gender>(gender, true, out _);

    private bool BeValidRole(string? role) =>
        Enum.TryParse<UserRole>(role, true, out _);
}