using Domain;
using FluentValidation;

namespace Application.Reservations.Update;

public sealed class UpdateReservationCommandValidator : AbstractValidator<UpdateReservationCommand>
{
    public UpdateReservationCommandValidator()
    {
        RuleFor(c => c.PassengerCount)
            .NotEmpty()
            .GreaterThan(0)
            .When(c => c.PassengerCount.HasValue);

        RuleFor(c => c.Status)
            .Must(BeValidStatus)
            .WithMessage("Status must be either Active, Canceled or Completed.")
            .When(c => !string.IsNullOrWhiteSpace(c.Status));
    }

    private bool BeValidStatus(string? status) =>
        Enum.TryParse<ReservationStatus>(status, true, out _);
}
