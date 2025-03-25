using Application.Abstractions.Messaging;

namespace Application.Flights.Create;

public sealed record CreateFlightCommand(
    Guid AirlineId,
    string Departure,
    string Destination,
    string DepartureTime,
    string ArrivalTime,
    int AvailableSeats,
    decimal Price) : ICommand<Guid>;