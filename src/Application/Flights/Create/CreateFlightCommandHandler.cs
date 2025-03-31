using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Flights;
using SharedKernel;

namespace Application.Flights.Create;

public sealed class CreateFlightCommandHandler(IApplicationDbContext context)
    : ICommandHandler<CreateFlightCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateFlightCommand command, CancellationToken cancellationToken)
    {
        var flight = command.ToFlight();

        flight.Raise(new FlightCreatedDomainEvent(flight.Id, flight.Departure, flight.Destination));

        await context.Flights.AddAsync(flight, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return flight.Id;
    }
}