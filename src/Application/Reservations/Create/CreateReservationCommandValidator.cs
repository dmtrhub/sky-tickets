using FluentValidation;

namespace Application.Reservations.Create;

internal sealed class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator()
    {
        RuleFor(c => c.FlightId)
            .NotEmpty()
            .Must(BeValidGuid)
            .WithMessage("Flight id must be a valid GUID.");

        RuleFor(c => c.PassengerCount)
            .NotEmpty()
            .GreaterThan(0);
    }

    private bool BeValidGuid(Guid flightId) =>
        flightId != Guid.Empty;
}
