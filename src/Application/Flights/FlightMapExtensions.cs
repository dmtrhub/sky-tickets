using Application.Flights.Create;
using Application.Flights.Update;
using Domain;
using Domain.Flights;
using System.Globalization;

namespace Application.Flights;

public static class FlightMapExtensions
{
    public static FlightResponse ToFlightResponse(this Flight flight) =>
        new()
        {
            Id = flight.Id,
            AirlineId = flight.AirlineId,
            Departure = flight.Departure,
            Destination = flight.Destination,
            DepartureTime = flight.DepartureTime,
            ArrivalTime = flight.ArrivalTime,
            AvailableSeats = flight.AvailableSeats,
            BookedSeats = flight.BookedSeats,
            Price = flight.Price,
            Status = flight.Status.ToString(),
            //Reservations = flight.Reservations
        };

    public static Flight ToFlight(this CreateFlightCommand command) =>
        new()
        {
            Id = Guid.NewGuid(),
            AirlineId = command.AirlineId,
            Departure = command.Departure,
            Destination = command.Destination,
            DepartureTime = DateTime.ParseExact(command.DepartureTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
            ArrivalTime = DateTime.ParseExact(command.ArrivalTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
            AvailableSeats = command.AvailableSeats,
            BookedSeats = 0,
            Price = command.Price
        };

    public static void UpdateFlight(this Flight flight, UpdateFlightCommand command)
    {
        if (!string.IsNullOrEmpty(command.Departure))
            flight.Departure = command.Departure;

        if(!string.IsNullOrEmpty(command.Destination))
            flight.Destination = command.Destination;

        if (!string.IsNullOrEmpty(command.DepartureTime))
            flight.DepartureTime = DateTime.ParseExact(command.DepartureTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

        if (!string.IsNullOrEmpty(command.ArrivalTime))
            flight.ArrivalTime = DateTime.ParseExact(command.ArrivalTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

        if (!string.IsNullOrEmpty(command.Status))
            flight.Status = Enum.Parse<FlightStatus>(command.Status, true);

        if (command.AvailableSeats.HasValue)
            flight.AvailableSeats = command.AvailableSeats.Value;

        if (command.BookedSeats.HasValue)
            flight.BookedSeats = command.BookedSeats.Value;

        if (command.Price.HasValue)
            flight.Price = command.Price.Value;
    }
}
