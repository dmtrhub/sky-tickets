using Application.Abstractions.Messaging;

namespace Application.Flights.SearchActive;

public sealed record SearchActiveFlightsQuery(string Destination, DateTime DepartureTime) : IQuery<List<FlightResponse>>;
