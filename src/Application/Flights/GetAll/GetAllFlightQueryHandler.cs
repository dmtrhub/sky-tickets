using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Flights.GetAll;

public sealed class GetAllFlightQueryHandler(IApplicationDbContext context) 
    : IQueryHandler<GetAllFlightsQuery, List<FlightResponse>>
{
    public async Task<Result<List<FlightResponse>>> Handle(GetAllFlightsQuery query, CancellationToken cancellationToken)
    {
        var flights = await context.Flights
            .Select(f => f.ToFlightResponse())
            .ToListAsync(cancellationToken);

        if (flights is null)
            return Result.Failure<List<FlightResponse>>(FlightErrors.NoFlightsFound);

        return flights;
    }
}