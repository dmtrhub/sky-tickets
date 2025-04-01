using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Flights;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Flights.AdvancedSearchActive;

public sealed class AdvancedSearchActiveFlightsQueryHandler(IApplicationDbContext context)
    : IQueryHandler<AdvancedSearchActiveFlightsQuery, List<FlightResponse>>
{
    public async Task<Result<List<FlightResponse>>> Handle(AdvancedSearchActiveFlightsQuery query, CancellationToken cancellationToken)
    {
        var flights = context.Flights.AsQueryable();

        if (!string.IsNullOrEmpty(query.Destination))
            flights = flights.Where(f => f.Destination.Contains(query.Destination));

        if (query.Date.HasValue)
            flights = flights.Where(f => f.DepartureTime.Date == query.Date.Value.Date);

        if (query.MinSeatsAvailable.HasValue)
            flights = flights.Where(f => f.AvailableSeats >= query.MinSeatsAvailable.Value);

        if (query.MaxPrice.HasValue)
            flights = flights.Where(f => f.Price <= query.MaxPrice.Value);

        flights = flights.Where(f => f.Status == Domain.FlightStatus.Active);

        var result = await flights
            .Include(f => f.Airline)
            .Include(f => f.Reservations)
            .ThenInclude(r => r.User)
            .Select(f => f.ToFlightResponse())
            .ToListAsync(cancellationToken);

        if (result is null)
            return Result.Failure<List<FlightResponse>>(FlightErrors.NoFlightsFound);

        return result;
    }
}
