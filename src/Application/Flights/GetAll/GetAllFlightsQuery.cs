using Application.Abstractions.Messaging;

namespace Application.Flights.GetAll;

public sealed record GetAllFlightsQuery() : IQuery<List<FlightResponse>>;