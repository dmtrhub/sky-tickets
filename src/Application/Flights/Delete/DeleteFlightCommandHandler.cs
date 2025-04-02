using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain;
using Domain.Flights;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Flights.Delete;

public sealed class DeleteFlightCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteFlightCommand, Guid>
{
    public async Task<Result<Guid>> Handle(DeleteFlightCommand command, CancellationToken cancellationToken)
    {
        var flight = await context.Flights
            .Where(f => f.Id == command.Id)
            .Include(f => f.Reservations)
            .FirstOrDefaultAsync(cancellationToken);

        if (flight is null)
            return Result.Failure<Guid>(FlightErrors.NotFound(command.Id));

        bool hasReservations = flight.Reservations.Any(r => r.Status == ReservationStatus.Created
            || r.Status == ReservationStatus.Approved);

        if (hasReservations)
            return Result.Failure<Guid>(FlightErrors.HasReservations);

        flight.Raise(new FlightDeletedDomainEvent(flight.Id, flight.Departure, flight.Destination));

        context.Flights.Remove(flight);
        await context.SaveChangesAsync(cancellationToken);

        return flight.Id;
    }
}