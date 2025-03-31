using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Flights;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Flights.GetById;

public sealed class GetFlightByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetFlightByIdQuery, FlightResponse>
{
    public async Task<Result<FlightResponse>> Handle(GetFlightByIdQuery query, CancellationToken cancellationToken)
    {
        var flight = await context.Flights
            .Where(f => f.Id == query.Id)
            .Include(f => f.Airline)
            .Include(f => f.Reservations)
            .ThenInclude(r => r.User)
            .Select(f => f.ToFlightResponse())
            .FirstOrDefaultAsync(cancellationToken);

        if(flight is null)
            return Result.Failure<FlightResponse>(FlightErrors.NotFound(query.Id));

        return flight;
    }
}