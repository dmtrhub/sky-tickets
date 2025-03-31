using Application.Flights;

namespace Application.Airlines;

public sealed record AirlineResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Address { get; init; }
    public string ContactInfo { get; init; }
    public List<FlightResponse> ActiveFlights { get; init; } 
}
