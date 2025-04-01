using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Flights;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Flights.Update;

public sealed class UpdateFlightCommandHandler(IApplicationDbContext context)
    : ICommandHandler<UpdateFlightCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateFlightCommand command, CancellationToken cancellationToken)
    {
        var flight = await context.Flights
            .Where(f => f.Id == command.Id)
            .Include(f => f.Reservations)
            .FirstOrDefaultAsync(cancellationToken);

        if (flight is null)
            return Result.Failure<Guid>(FlightErrors.NotFound(command.Id));

        bool hasReservations = flight.Reservations.Any(r => r.Status == Domain.ReservationStatus.Created 
            || r.Status == Domain.ReservationStatus.Approved);

        flight.UpdateFlight(command, hasReservations);

        flight.Raise(new FlightUpdatedDomainEvent(flight.Id, flight.Departure, flight.Destination));

        await context.SaveChangesAsync(cancellationToken);

        return flight.Id;
    }
}