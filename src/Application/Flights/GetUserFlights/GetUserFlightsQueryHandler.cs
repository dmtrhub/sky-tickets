using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain;
using Domain.Flights;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Security.Claims;

namespace Application.Flights.GetUserFlights;

public sealed class GetUserFlightsQueryHandler(
    IApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor)
    : IQueryHandler<GetUserFlightsQuery, List<FlightResponse>>
{
    public async Task<Result<List<FlightResponse>>> Handle(GetUserFlightsQuery query, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            return Result.Failure<List<FlightResponse>>(UserErrors.Unauthenticated);

        var flights = await context.Flights
            .Where(f => f.Status == FlightStatus.Active ||
                f.Reservations.Any(r => r.UserId == parsedUserId))
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
