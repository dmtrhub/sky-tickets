using Application.Flights.Create;
using Application.Flights.SearchActive;
using Application.Flights.Update;
using Application.Reservations;
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
            Departure = flight.Departure,
            Destination = flight.Destination,
            DepartureTime = flight.DepartureTime,
            ArrivalTime = flight.ArrivalTime,
            AvailableSeats = flight.AvailableSeats,
            BookedSeats = flight.BookedSeats,
            Price = flight.Price,
            Status = flight.Status.ToString(),
            Reservations = flight.Reservations.Where(r => r.Status == ReservationStatus.Approved).Select(r => r.ToReservationResponse()).ToList(),
            AirlineName = flight.Airline.Name
        };

    public static Flight ToFlight(this CreateFlightCommand command) =>
        Flight.Create(command.AirlineId,
            command.Departure,
            command.Destination,
            DateTime.ParseExact(command.DepartureTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
            DateTime.ParseExact(command.ArrivalTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
            command.AvailableSeats,
            command.Price);

    public static void UpdateFlight(this Flight flight, UpdateFlightCommand command, bool hasReservations)
    {
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

        if (command.Price.HasValue && !hasReservations)
            flight.Price = command.Price.Value;
    }

    public static IQueryable<Flight> SearchFlights(this IQueryable<Flight> flights, SearchActiveFlightsQuery query)
    {
        if (!string.IsNullOrEmpty(query.Destination))
            flights = flights.Where(f => f.Destination.Contains(query.Destination));

        if (query.Date.HasValue)
            flights = flights.Where(f => f.DepartureTime.Date == query.Date.Value.Date);

        if (query.MinSeatsAvailable.HasValue)
            flights = flights.Where(f => f.AvailableSeats >= query.MinSeatsAvailable.Value);

        if (query.MaxPrice.HasValue)
            flights = flights.Where(f => f.Price <= query.MaxPrice.Value);

        flights = flights.Where(f => f.Status == Domain.FlightStatus.Active);

        return flights;
    }
}
