using FluentValidation;
using System.Globalization;

namespace Application.Flights.Create;

public sealed class CreateFlightCommandValidator : AbstractValidator<CreateFlightCommand>
{
    private const string DateTimeFormat = "yyyy-MM-dd HH:mm";
    public CreateFlightCommandValidator()
    {
        RuleFor(c => c.AirlineId)
            .NotEmpty()
            .Must(BeValidGuid)
            .WithMessage("Airline id must be a valid GUID.");

        RuleFor(c => c.Departure)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.Destination)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.DepartureTime)
            .NotEmpty()
            .Must(BeValidDateTime)
            .WithMessage($"Departure time must be in format {DateTimeFormat}.")
            .LessThan(c => c.ArrivalTime)
            .WithMessage("Departure time must be before arrival time.")
            .GreaterThan(DateTime.UtcNow.ToString(DateTimeFormat))
            .WithMessage("Departure time must be in the future.");

        RuleFor(c => c.ArrivalTime)
            .NotEmpty()
            .Must(BeValidDateTime)
            .WithMessage($"Arrival time must be in format {DateTimeFormat}.")
            .GreaterThan(c => c.DepartureTime)
            .WithMessage("Arrival time must be after departure time.");

        RuleFor(c => c.AvailableSeats)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(c => c.Price)
            .NotEmpty()
            .GreaterThan(0);
    }

    private bool BeValidDateTime(string dateTime) =>
        DateTime.TryParseExact(dateTime, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);

    private bool BeValidGuid(Guid airlineId) =>
        airlineId != Guid.Empty;
}
