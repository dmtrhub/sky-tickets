using Application.Airlines.Create;
using Application.Airlines.Update;
using Application.Flights;
using Domain.Airlines;
using Domain;
using Application.Reviews;
using Application.Airlines.SearchAirlines;
using Microsoft.EntityFrameworkCore;

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
            ActiveFlights = airline.Flights.Where(f => f.Status == FlightStatus.Active).Select(f => f.ToFlightResponse()).ToList(),
            ApprovedReviews = airline.Reviews.Where(r => r.Status == ReviewStatus.Approved).Select(r => r.ToReviewResponse()).ToList()
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

    public static IQueryable<Airline> SearchAirlines(this IQueryable<Airline> airlines, SearchAirlinesQuery query)
    {
        if (!string.IsNullOrWhiteSpace(query.Name))
            airlines = airlines.Where(a => EF.Functions.Like(a.Name, $"%{query.Name}%"));

        if (!string.IsNullOrWhiteSpace(query.Address))
            airlines = airlines.Where(a => EF.Functions.Like(a.Address, $"%{query.Address}%"));

        return airlines;
    }
}
