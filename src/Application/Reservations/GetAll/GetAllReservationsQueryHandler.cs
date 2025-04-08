using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Reservations;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reservations.GetAll;

public sealed class GetAllReservationsQueryHandler(IRepository<Reservation> reservationRepository) 
    : IQueryHandler<GetAllReservationsQuery, List<ReservationResponse>>
{
    public async Task<Result<List<ReservationResponse>>> Handle(GetAllReservationsQuery query, CancellationToken cancellationToken)
    {
        var reservationQuery = await reservationRepository.AsQueryable();

        var reservations = await reservationQuery
            .Include(r => r.User)
            .Include(r => r.Flight)
            .ThenInclude(f => f.Airline)
            .Select(r => r.ToReservationResponse())
            .ToListAsync(cancellationToken);

        if(reservations.Count == 0)
            return Result.Failure<List<ReservationResponse>>(ReservationErrors.NoReservationsFound);

        return reservations;
    }
}
