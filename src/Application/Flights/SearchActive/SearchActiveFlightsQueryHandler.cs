using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain;
using Domain.Flights;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Flights.SearchActive;

public sealed class SearchActiveFlightsQueryHandler(IRepository<Flight> flightRepository)
    : IQueryHandler<SearchActiveFlightsQuery, List<FlightResponse>>
{
    public async Task<Result<List<FlightResponse>>> Handle(SearchActiveFlightsQuery query, CancellationToken cancellationToken)
    {
        var flightsQuery = await flightRepository
            .AsQueryable();

        var flights = await flightsQuery
            .SearchFlights(query)
            .Include(f => f.Airline)
            .Include(f => f.Reservations)
            .ThenInclude(r => r.User)
            .Select(f => f.ToFlightResponse())
            .ToListAsync(cancellationToken);

        if (flights.Count == 0)
            return Result.Failure<List<FlightResponse>>(FlightErrors.NoFlightsFound);

        return flights;
    }
}
