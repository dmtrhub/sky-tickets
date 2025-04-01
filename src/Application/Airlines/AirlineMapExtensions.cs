using Application.Airlines.Create;
using Application.Airlines.Update;
using Application.Flights;
using Domain.Airlines;
using Domain;
using Application.Reviews;

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
            ActiveFlights = airline.Flights.Where(f => f.Status is FlightStatus.Active).Select(f => f.ToFlightResponse()).ToList(),
            ApprovedReviews = airline.Reviews.Where(r => r.Status is ReviewStatus.Approved).Select(r => r.ToReviewResponse()).ToList()
        };

    public static Airline ToAirline(this CreateAirlineCommand command) =>
        Airline.Create(command.Name, command.Address, command.ContactInfo);

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
