using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Flights;
using SharedKernel;

namespace Application.Flights.Update;

public sealed class UpdateFlightCommandHandler(IApplicationDbContext context)
    : ICommandHandler<UpdateFlightCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateFlightCommand command, CancellationToken cancellationToken)
    {
        var flight = await context.Flights.FindAsync([command.Id], cancellationToken);

        if(flight is null)
            return Result.Failure<Guid>(FlightErrors.NotFound(command.Id));

        flight.UpdateFlight(command);

        flight.Raise(new FlightUpdatedDomainEvent(flight.Id, flight.Departure, flight.Destination));

        await context.SaveChangesAsync(cancellationToken);

        return flight.Id;
    }
}