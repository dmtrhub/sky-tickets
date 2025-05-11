using Domain;
using Domain.Flights;
using Domain.Reservations;
using Domain.Users;

namespace Application.UnitTests.Builders;

public class ReservationBuilder
{
    private Guid _id = Guid.NewGuid();
    private Flight _flight = new FlightBuilder().Build();
    private readonly User _user = new UserBuilder().Build();
    private int _passengerCount = 15;
    private readonly decimal _totalPrice;
    private ReservationStatus _status = ReservationStatus.Created; // Created, Approved, Canceled, Completed

    public ReservationBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public ReservationBuilder WithUserId(Guid userId)
    {
        _user.Id = userId;
        return this;
    }

    public ReservationBuilder WithFlight(Flight flight)
    {
        _flight = flight;
        return this;
    }

    public ReservationBuilder WithPassengerCount(int passengerCount)
    {
        _passengerCount = passengerCount;
        return this;
    }

    public ReservationBuilder WithStatus(ReservationStatus status)
    {
        _status = status;
        return this;
    }

    public Reservation Build()
    {
        var reservation = Reservation.Create(
            userId: _user.Id, 
            flightId: _flight.Id, 
            passengerCount: _passengerCount);

        typeof(Reservation)
            .GetProperty(nameof(Reservation.Id))
            ?.SetValue(reservation, _id);

        typeof(Reservation)
            .GetProperty(nameof(Reservation.TotalPrice))
            ?.SetValue(reservation, _totalPrice);

        typeof(Reservation)
            .GetProperty(nameof(Reservation.Status))
            ?.SetValue(reservation, _status);

        typeof(Reservation)
            .GetProperty(nameof(Reservation.Flight))
            ?.SetValue(reservation, _flight);

        typeof(Reservation)
            .GetProperty(nameof(Reservation.User))
            ?.SetValue(reservation, _user);

        return reservation;
    }
}
