using Application.Airlines.Create;
using Application.Airlines.Update;
using Application.Flights;
using Domain.Airlines;

namespace Application.Airlines;

public static class AirlineMapExtensions
{
    public static AirlineResponse ToAirlineResponse(this Airline airline) =>
        new()
        {
            Id = airline.Id,
            Name = airline.Name,
            Address = airline.Address,
            ContactInfo = airline.ContactInfo,
            Flights = airline.Flights.Select(f => f.ToFlightResponse()).ToList(),
            //Reviews = airline.Reviews
        };

    public static Airline ToAirline(this CreateAirlineCommand command) =>
        new()
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Address = command.Address,
            ContactInfo = command.ContactInfo,
            Flights = [],
            Reviews = []
        };

    public static void UpdateAirline(this Airline airline, UpdateAirlineCommand command)
    {
        if (!string.IsNullOrWhiteSpace(command.Name))
            airline.Name = command.Name;

        if (!string.IsNullOrWhiteSpace(command.Address))
            airline.Address = command.Address;

        if (!string.IsNullOrWhiteSpace(command.ContactInfo))
            airline.ContactInfo = command.ContactInfo;
    }
}
