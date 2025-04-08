using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain;
using Domain.Flights;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Flights.Delete;

public sealed class DeleteFlightCommandHandler(
    IRepository<Flight> flightRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteFlightCommand, Guid>
{
    public async Task<Result<Guid>> Handle(DeleteFlightCommand command, CancellationToken cancellationToken)
    {
        var flight = await flightRepository.GetByIdAsync(command.Id, cancellationToken);

        if (flight is null)
            return Result.Failure<Guid>(FlightErrors.NotFound(command.Id));

        bool hasReservations = flight.Reservations.Any(r => r.Status == ReservationStatus.Created
            || r.Status == ReservationStatus.Approved);

        if (hasReservations)
            return Result.Failure<Guid>(FlightErrors.HasReservations);

        flight.Raise(new FlightDeletedDomainEvent(flight.Id, flight.Departure, flight.Destination));

        flightRepository.Remove(flight);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return flight.Id;
    }
}