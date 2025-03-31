using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Flights;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Flights.GetAll;

public sealed class GetAllFlightsQueryHandler(IApplicationDbContext context) 
    : IQueryHandler<GetAllFlightsQuery, List<FlightResponse>>
{
    public async Task<Result<List<FlightResponse>>> Handle(GetAllFlightsQuery query, CancellationToken cancellationToken)
    {
        var flights = await context.Flights
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