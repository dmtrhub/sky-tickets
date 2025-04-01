using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Flights;
using Domain;
using SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Application.Flights.SearchActive;

public sealed class SearchActiveFlightsQueryHandler(IApplicationDbContext context)
    : IQueryHandler<SearchActiveFlightsQuery, List<FlightResponse>>
{
    public async Task<Result<List<FlightResponse>>> Handle(SearchActiveFlightsQuery query, CancellationToken cancellationToken)
    {
        var flights = await context.Flights
            .Where(f => f.Destination.Contains(query.Destination)
                && f.DepartureTime == query.DepartureTime
                && f.Status == FlightStatus.Active)
            .Include(f => f.Airline)
            .Include(f => f.Reservations)
            .ThenInclude(r => r.User)
            .Select(f => f.ToFlightResponse())
            .ToListAsync(cancellationToken);

        if (flights is null)
            return Result.Failure<List<FlightResponse>>(FlightErrors.NoFlightsFound);

        return flights;
    }
}