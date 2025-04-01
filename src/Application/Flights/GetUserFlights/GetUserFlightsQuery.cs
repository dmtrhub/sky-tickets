using Application.Abstractions.Messaging;

namespace Application.Flights.GetUserFlights;

public sealed record GetUserFlightsQuery() : IQuery<List<FlightResponse>>;
