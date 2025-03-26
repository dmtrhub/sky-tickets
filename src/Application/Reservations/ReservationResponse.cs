namespace Application.Reservations;

public sealed record ReservationResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid FlightId { get; init; }
    public int PassengerCount { get; init; }
    public decimal TotalPrice { get; init; }
    public string Status { get; init; }
}
