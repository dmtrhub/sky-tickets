using Application.Reservations.Create;
using Application.Reservations.Update;
using Domain;
using Domain.Reservations;

namespace Application.Reservations;

public static class ReservationMapExtensions
{
    public static ReservationResponse ToReservationResponse(this Reservation reservation) =>
        new()
        {
            Id = reservation.Id,
            PassengerCount = reservation.PassengerCount,
            TotalPrice = reservation.TotalPrice,
            Status = reservation.Status.ToString(),
            FirstName = reservation.User.FirstName,
            LastName = reservation.User.LastName,
            Departure = reservation.Flight.Departure,
            Destination = reservation.Flight.Destination,
            DepartureTime = reservation.Flight.DepartureTime,
            AirlineName = reservation.Flight.Airline.Name
        };

    public static Reservation ToReservation(this CreateReservationCommand command, Guid userId, decimal totalPrice) =>
        Reservation.Create(userId, command.FlightId, command.PassengerCount, totalPrice);

    public static void UpdateReservation(this Reservation reservation, UpdateReservationCommand command)
    {
        if (command.PassengerCount.HasValue)
        {
            reservation.PassengerCount = command.PassengerCount.Value;
            reservation.TotalPrice = reservation.Flight.Price * command.PassengerCount.Value;
        }
        if (!string.IsNullOrEmpty(command.Status))
            reservation.Status = Enum.Parse<ReservationStatus>(command.Status, true);
    }
}
