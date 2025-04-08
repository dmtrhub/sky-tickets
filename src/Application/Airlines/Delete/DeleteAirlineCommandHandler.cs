using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain;
using Domain.Airlines;
using SharedKernel;

namespace Application.Airlines.Delete;

public sealed class DeleteAirlineCommandHandler(
    IRepository<Airline> airlineRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteAirlineCommand, Guid>
{
    public async Task<Result<Guid>> Handle(DeleteAirlineCommand command, CancellationToken cancellationToken)
    {
        var airline = await airlineRepository.GetByIdAsync(command.Id, cancellationToken);

        if (airline is null)
            return Result.Failure<Guid>(AirlineErrors.NotFound(command.Id));

        bool hasActiveFlights = airline.Flights.Any(f => f.Status == FlightStatus.Active);
        if (hasActiveFlights)
            return Result.Failure<Guid>(AirlineErrors.CannotDeleteWithActiveFlights);

        airline.Raise(new AirlineDeletedDomainEvent(airline.Name));

        // Korišćenje DbContext za brisanje
        airlineRepository.Remove(airline);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return airline.Id;
    }
}