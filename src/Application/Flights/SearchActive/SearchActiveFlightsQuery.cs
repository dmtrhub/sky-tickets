using Application.Abstractions.Messaging;

namespace Application.Flights.SearchActive;

public sealed record SearchActiveFlightsQuery(
    string? Destination,
    DateTime? Date,
    int? MinSeatsAvailable,
    decimal? MaxPrice) : IQuery<List<FlightResponse>>;
