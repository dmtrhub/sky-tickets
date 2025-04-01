using Application.Abstractions.Messaging;

namespace Application.Flights.AdvancedSearchActive;

public sealed record AdvancedSearchActiveFlightsQuery(
    string? Destination,
    DateTime? Date,
    int? MinSeatsAvailable,
    decimal? MaxPrice
) : IQuery<List<FlightResponse>>;
