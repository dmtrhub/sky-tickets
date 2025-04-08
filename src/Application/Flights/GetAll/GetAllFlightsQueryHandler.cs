using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Flights;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Flights.GetAll;

public sealed class GetAllFlightsQueryHandler(
    IRepository<Flight> flightRepository) 
    : IQueryHandler<GetAllFlightsQuery, List<FlightResponse>>
{
    public async Task<Result<List<FlightResponse>>> Handle(GetAllFlightsQuery query, CancellationToken cancellationToken)
    {
        var flightQuery = await flightRepository.AsQueryable();

        var flights = await flightQuery
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