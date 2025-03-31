using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Reservations;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reservations.GetById;

public sealed class GetReservationByIdQueryHandler(IApplicationDbContext context) 
    : IQueryHandler<GetReservationByIdQuery, ReservationResponse>
{
    public async Task<Result<ReservationResponse>> Handle(GetReservationByIdQuery query, CancellationToken cancellationToken)
    {
        var reservation = await context.Reservations
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
