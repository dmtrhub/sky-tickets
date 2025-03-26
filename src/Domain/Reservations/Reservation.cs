using Domain.Flights;
using Domain.Users;
using SharedKernel;

namespace Domain.Reservations;

public sealed class Reservation : Entity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid FlightId { get; set; }
    public Flight Flight { get; set; }
    public int PassengerCount { get; set; }
    public decimal TotalPrice { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Created; // Created, Approved, Canceled, Completed
}
