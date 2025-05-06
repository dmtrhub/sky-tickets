using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Flights;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Flights.Update;

public sealed class UpdateFlightCommandHandler(
    IRepository<Flight> flightRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateFlightCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateFlightCommand command, CancellationToken cancellationToken)
    {
        var flightQuery = await flightRepository.AsQueryable();

        var flight = await flightQuery
            .Where(f => f.Id == command.Id)
            .Include(f => f.Reservations)
            .FirstOrDefaultAsync(cancellationToken);

        if (flight is null)
            return Result.Failure<Guid>(FlightErrors.NotFound(command.Id));

        bool hasReservations = flight.Reservations.Any(r => r.Status == Domain.ReservationStatus.Created
            || r.Status == Domain.ReservationStatus.Approved);

        flight.UpdateFlight(command, hasReservations);
        flight.Raise(new FlightUpdatedDomainEvent(flight.Id, flight.Departure, flight.Destination));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return flight.Id;
    }
}