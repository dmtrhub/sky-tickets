using Domain;
using Domain.Airlines;
using Domain.Flights;
using Domain.Reservations;

namespace Application.UnitTests.Builders;

public class FlightBuilder
{
    private Guid _id = Guid.NewGuid();
    private readonly Airline _airline = new AirlineBuilder().Build();
    private readonly List<Reservation> _reservations = [];
    private readonly string _departure = "Belgrade";
    private string _destination = "London";
    private DateTime _departureTime = DateTime.UtcNow.AddDays(2);
    private readonly DateTime _arrivalTime = DateTime.UtcNow.AddDays(7);
    private int _availableSeats = 100;
    private decimal _price = 250;
    private FlightStatus _status = FlightStatus.Active; // Active, Canceled, Completed

    public FlightBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public FlightBuilder WithAvailableSeats(int availableSeats)
    {
        _availableSeats = availableSeats;
        return this;
    }

    public FlightBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    public FlightBuilder WithDestination(string destination)
    {
        _destination = destination;
        return this;
    }

    public FlightBuilder WithDepartureTime(DateTime departureTime)
    {
        _departureTime = departureTime;
        return this;
    }

    public FlightBuilder WithReservation(Reservation reservation)
    {
        _reservations.Add(reservation);
        reservation.Flight = this.Build();
        return this;
    }

    public FlightBuilder WithNoReservations()
    {
        _reservations.Clear();
        return this;
    }

    public FlightBuilder WithStatus(FlightStatus status)
    {
        _status = status;
        return this;
    }

    public Flight Build()
    {
        var flight = Flight.Create(
            _airline.Id,
            _departure,
            _destination,
            _departureTime,
            _arrivalTime,
            _availableSeats,
            _price);

        typeof(Flight)
            .GetProperty(nameof(Flight.Id))!
            .SetValue(flight, _id);

        typeof(Flight)
            .GetProperty(nameof(Flight.Airline))!
            .SetValue(flight, _airline);

        typeof(Flight)
            .GetProperty(nameof(Flight.Status))!
            .SetValue(flight, _status);

        typeof(Flight)
            .GetProperty(nameof(Flight.Reservations))!
            .SetValue(flight, _reservations);

        return flight;
    }
}

