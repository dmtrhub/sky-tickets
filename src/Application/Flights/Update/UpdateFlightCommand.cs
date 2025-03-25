using Application.Abstractions.Messaging;

namespace Application.Flights.Update;

public sealed record UpdateFlightCommand(
    Guid Id,
    string? Departure,
    string? Destination,
    string? DepartureTime,
    string? ArrivalTime,
    int? AvailableSeats,
    int? BookedSeats,
    decimal? Price,
    string? Status) : ICommand<Guid>;