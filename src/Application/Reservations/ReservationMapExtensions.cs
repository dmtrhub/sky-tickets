using Domain.Reservations;

namespace Application.Reservations;

public static class ReservationMapExtensions
{
    public static ReservationResponse ToReservationResponse(this Reservation reservation) =>
        new()
        {
            Id = reservation.Id,
            UserId = reservation.UserId,
            FlightId = reservation.FlightId,
            PassengerCount = reservation.PassengerCount,
            TotalPrice = reservation.TotalPrice,
            Status = reservation.Status.ToString()
        };
}
