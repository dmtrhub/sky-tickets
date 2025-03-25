using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain;
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
            .Select(f => f.ToFlightResponse())
            .SingleOrDefaultAsync(cancellationToken);

        if(flight is null)
            return Result.Failure<FlightResponse>(FlightErrors.NotFound(query.Id));

        return flight;
    }
}