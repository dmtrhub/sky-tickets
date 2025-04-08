using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Flights;
using SharedKernel;

namespace Application.Flights.Create;

public sealed class CreateFlightCommandHandler(
    IRepository<Flight> flightRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateFlightCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateFlightCommand command, CancellationToken cancellationToken)
    {
        var flight = command.ToFlight();
        flight.Raise(new FlightCreatedDomainEvent(flight.Id, flight.Departure, flight.Destination));

        await flightRepository.AddAsync(flight, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return flight.Id;
    }
}