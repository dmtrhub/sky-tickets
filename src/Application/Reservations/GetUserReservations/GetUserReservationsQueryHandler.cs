using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Extensions;
using Domain.Reservations;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reservations.GetUserReservations;

public sealed class GetUserReservationsQueryHandler(
    IApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor)
    : IQueryHandler<GetUserReservationsQuery, List<ReservationResponse>>
{
    public async Task<Result<List<ReservationResponse>>> Handle(GetUserReservationsQuery query, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.GetUserId();

        if (userId is null)
            return Result.Failure<List<ReservationResponse>>(UserErrors.Unauthenticated);

        var reservations = await context.Reservations
            .Where(r => r.UserId == userId)
            .Include(r => r.User)
            .Include(r => r.Flight)
            .ThenInclude(f => f.Airline)
            .Select(r => r.ToReservationResponse())
            .ToListAsync(cancellationToken);

        if (reservations is null)
            return Result.Failure<List<ReservationResponse>>(ReservationErrors.NoReservationsFound);

        return reservations;
    }
}

