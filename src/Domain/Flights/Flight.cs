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

    public Flight(
        Guid airlineId, 
        string departure, 
        string destination, 
        DateTime departureTime, 
        DateTime arrivalTime, 
        int availableSeats, 
        decimal price)
    {
        Id = Guid.NewGuid();    
        AirlineId = airlineId;
        Departure = departure;
        Destination = destination;
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
        AvailableSeats = availableSeats;
        BookedSeats = 0;
        Price = price;

    }

    public static Flight Create(
        Guid airlineId,
        string departure,
        string destination,
        DateTime departureTime,
        DateTime arrivalTime,
        int availableSeats,
        decimal price) =>
        new(
            airlineId,
            departure,
            destination,
            departureTime,
            arrivalTime,
            availableSeats,
            price);
}
