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

    public Reservation(Guid userId, Guid flightId, int passengerCount, decimal totalPrice)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        FlightId = flightId;
        PassengerCount = passengerCount;
        TotalPrice = totalPrice;
    }

    public static Reservation Create(Guid userId, Guid flightId, int passengerCount, decimal totalPrice) =>
        new(userId, flightId, passengerCount, totalPrice);
}
