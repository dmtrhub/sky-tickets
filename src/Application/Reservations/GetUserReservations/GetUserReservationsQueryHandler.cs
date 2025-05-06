using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Extensions;
using Domain.Reservations;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reservations.GetUserReservations;

public sealed class GetUserReservationsQueryHandler(
    IRepository<Reservation> reservationRepository,
    IHttpContextAccessor httpContextAccessor)
    : IQueryHandler<GetUserReservationsQuery, List<ReservationResponse>>
{
    public async Task<Result<List<ReservationResponse>>> Handle(GetUserReservationsQuery query, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.GetUserId();

        if (userId is null)
            return Result.Failure<List<ReservationResponse>>(UserErrors.Unauthenticated);

        var reservationQuery = await reservationRepository.AsQueryable();

        var reservations = await reservationQuery
            .Where(r => r.UserId == userId)
            .Include(r => r.User)
            .Include(r => r.Flight)
            .ThenInclude(f => f.Airline)
            .Select(r => r.ToReservationResponse())
            .ToListAsync(cancellationToken);

        if (reservations.Count == 0)
            return Result.Failure<List<ReservationResponse>>(ReservationErrors.NoReservationsFound);

        return reservations;
    }
}

