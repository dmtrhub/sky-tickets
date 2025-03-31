using Application.Reservations;

namespace Application.Flights;

public sealed record FlightResponse
{
    public Guid Id { get; init; }
    public string Departure { get; init; }
    public string Destination { get; init; }
    public DateTime DepartureTime { get; init; }
    public DateTime ArrivalTime { get; init; }
    public int AvailableSeats { get; init; }
    public int BookedSeats { get; init; }
    public decimal Price { get; init; }
    public string Status { get; init; }
    public List<ReservationResponse> Reservations { get; init; }
    public string AirlineName { get; init; }
}
