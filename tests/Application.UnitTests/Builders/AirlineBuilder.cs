using Domain.Airlines;
using Domain.Flights;
using Domain.Reviews;

namespace Application.UnitTests.Builders;

public class AirlineBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _name = "Test Airline";
    private string _address = "Test Address";
    private string _contact = "Test Contact";
    private readonly List<Flight> _flights = [];
    private readonly List<Review> _reviews = [];

    public AirlineBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public AirlineBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public AirlineBuilder WithAddress(string address)
    {
        _address = address;
        return this;
    }

    public AirlineBuilder WithContact(string contact)
    {
        _contact = contact;
        return this;
    }

    public AirlineBuilder WithFlight(Flight flight)
    {
        _flights.Add(flight);
        return this;
    }

    public AirlineBuilder WithReview(Review review)
    {
        _reviews.Add(review);
        return this;
    }

    public Airline Build()
    {
        var airline = Airline.Create(
            _name, 
            _address, 
            _contact);

        typeof(Airline)
            .GetProperty(nameof(Airline.Id))
            ?.SetValue(airline, _id);

        typeof(Airline)
            .GetProperty(nameof(Airline.Flights))
            ?.SetValue(airline, _flights);

        typeof(Airline)
            .GetProperty(nameof(Airline.Reviews))
            ?.SetValue(airline, _reviews);

        return airline;
    }
}
