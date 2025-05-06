using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Flights;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Flights.GetById;

public sealed class GetFlightByIdQueryHandler(
    IRepository<Flight> flightRepository)
    : IQueryHandler<GetFlightByIdQuery, FlightResponse>
{
    public async Task<Result<FlightResponse>> Handle(GetFlightByIdQuery query, CancellationToken cancellationToken)
    {
        var flightQuery = await flightRepository.AsQueryable();

        var flight = await flightQuery
            .Where(f => f.Id == query.Id)
            .Include(f => f.Airline)
            .Include(f => f.Reservations)
            .ThenInclude(r => r.User)
            .Select(f => f.ToFlightResponse())
            .FirstOrDefaultAsync(cancellationToken);

        if (flight is null)
            return Result.Failure<FlightResponse>(FlightErrors.NotFound(query.Id));

        return flight;
    }
}