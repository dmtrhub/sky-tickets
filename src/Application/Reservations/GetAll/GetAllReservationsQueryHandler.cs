using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Reservations;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reservations.GetAll;

public sealed class GetAllReservationsQueryHandler(IApplicationDbContext context) 
    : IQueryHandler<GetAllReservationsQuery, List<ReservationResponse>>
{
    public async Task<Result<List<ReservationResponse>>> Handle(GetAllReservationsQuery query, CancellationToken cancellationToken)
    {
        var reservations = await context.Reservations
            .Include(r => r.User)
            .Include(r => r.Flight)
            .ThenInclude(f => f.Airline)
            .Select(r => r.ToReservationResponse())
            .ToListAsync(cancellationToken);

        if(reservations is null)
        {
            return Result.Failure<List<ReservationResponse>>(ReservationErrors.NoReservationsFound);
        }

        return reservations;
    }
}
