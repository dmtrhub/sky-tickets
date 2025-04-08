using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Reservations;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reservations.GetById;

public sealed class GetReservationByIdQueryHandler(IRepository<Reservation> reservationRepository) 
    : IQueryHandler<GetReservationByIdQuery, ReservationResponse>
{
    public async Task<Result<ReservationResponse>> Handle(GetReservationByIdQuery query, CancellationToken cancellationToken)
    {
        var reservationQuery = await reservationRepository.AsQueryable();

        var reservation = await reservationQuery
            .Where(r => r.Id == query.Id)
            .Include(r => r.User)
            .Include(r => r.Flight)
            .ThenInclude(f => f.Airline)
            .Select(r => r.ToReservationResponse())
            .FirstOrDefaultAsync(cancellationToken);

        if (reservation is null)
            return Result.Failure<ReservationResponse>(ReservationErrors.NotFound(query.Id));

        return reservation;
    }
}
