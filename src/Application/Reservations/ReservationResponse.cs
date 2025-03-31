namespace Application.Reservations;

public sealed record ReservationResponse
{
    public Guid Id { get; init; }
    public int PassengerCount { get; init; }
    public decimal TotalPrice { get; init; }
    public string Status { get; init; }

    // User
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Flight
    public string Departure { get; set; }
    public string Destination { get; set; }
    public DateTime DepartureTime { get; set; }

    // Airline
    public string AirlineName { get; set; }
}
