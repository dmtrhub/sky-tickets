using Domain.Airlines;
using Domain.Reservations;
using SharedKernel;

namespace Domain.Flights;

public sealed class Flight : Entity
{
    public Guid Id { get; set; }
    public Guid AirlineId { get; set; }
    public Airline Airline { get; set; }
    public string Departure { get; set; }
    public string Destination { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public int AvailableSeats { get; set; }
    public int BookedSeats { get; set; }
    public decimal Price { get; set; }
    public FlightStatus Status { get; set; } = FlightStatus.Active; // Active, Canceled, Completed
    public List<Reservation> Reservations { get; set; } = [];
}
