using Application.Abstractions.Messaging;

namespace Application.Airlines.SearchAirlines;

public sealed record SearchAirlinesQuery(string? Name, string? Address) : IQuery<List<AirlineResponse>>;
