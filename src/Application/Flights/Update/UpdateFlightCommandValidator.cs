﻿using Domain;
using FluentValidation;
using System.Globalization;

namespace Application.Flights.Update;

internal sealed class UpdateFlightCommandValidator : AbstractValidator<UpdateFlightCommand>
{
    private const string DateTimeFormat = "yyyy-MM-dd HH:mm";
    public UpdateFlightCommandValidator()
    {
        RuleFor(c => c.Departure)
            .MaximumLength(50)
            .When(c => !string.IsNullOrWhiteSpace(c.Departure));

        RuleFor(c => c.Destination)
            .MaximumLength(50)
            .When(c => !string.IsNullOrWhiteSpace(c.Destination));

        RuleFor(c => c.DepartureTime)
            .Must(BeValidDateTime)
            .WithMessage($"Departure time must be in format {DateTimeFormat}.")
            .LessThan(c => c.ArrivalTime)
            .WithMessage("Departure time must be before arrival time.")
            .GreaterThan(DateTime.UtcNow.ToString(DateTimeFormat))
            .WithMessage("Departure time must be in the future.")
            .When(c => !string.IsNullOrWhiteSpace(c.DepartureTime));

        RuleFor(c => c.ArrivalTime)
            .Must(BeValidDateTime)
            .WithMessage($"Arrival time must be in format {DateTimeFormat}.")
            .GreaterThan(c => c.DepartureTime)
            .WithMessage("Arrival time must be after departure time.")
            .When(c => !string.IsNullOrWhiteSpace(c.ArrivalTime));

        RuleFor(c => c.AvailableSeats)
            .GreaterThanOrEqualTo(0)
            .When(c => c.AvailableSeats.HasValue);

        RuleFor(c => c.Price)
            .GreaterThan(0)
            .When(c => c.Price.HasValue);

        RuleFor(c => c.BookedSeats)
            .GreaterThanOrEqualTo(0)
            .When(c => c.BookedSeats.HasValue);

        RuleFor(c => c.Status)
            .Must(BeValidStatus)
            .WithMessage("Status must be either Active, Canceled or Completed.")
            .When(c => !string.IsNullOrWhiteSpace(c.Status));
    }

    private bool BeValidDateTime(string? dateTime) =>
        DateTime.TryParseExact(dateTime, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);

    private bool BeValidStatus(string? status) =>
        Enum.TryParse<FlightStatus>(status, true, out _);
}
