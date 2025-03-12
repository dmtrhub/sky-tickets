using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain;

public sealed class Reservation
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
