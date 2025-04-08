using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Extensions;
using Domain;
using Domain.Flights;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Flights.GetUserFlights;

public sealed class GetUserFlightsQueryHandler(
    IRepository<Flight> flightRepository,
    IHttpContextAccessor httpContextAccessor) : IQueryHandler<GetUserFlightsQuery, List<FlightResponse>>
{
    public async Task<Result<List<FlightResponse>>> Handle(GetUserFlightsQuery query, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.GetUserId();
        if (userId is null)
            return Result.Failure<List<FlightResponse>>(UserErrors.Unauthenticated);

        var flightsQuery = await flightRepository.AsQueryable();

        var flights = await flightsQuery
            .Where(f => f.Status == FlightStatus.Active ||
                f.Reservations.Any(r => r.UserId == userId))
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
