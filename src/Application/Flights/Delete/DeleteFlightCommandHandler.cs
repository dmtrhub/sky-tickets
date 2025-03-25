using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain;
using SharedKernel;

namespace Application.Flights.Delete;

public sealed class DeleteFlightCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteFlightCommand, Guid>
{
    public async Task<Result<Guid>> Handle(DeleteFlightCommand command, CancellationToken cancellationToken)
    {
        var flight = await context.Flights.FindAsync([command.Id], cancellationToken);

        if (flight is null)
            return Result.Failure<Guid>(FlightErrors.NotFound(command.Id));

        context.Flights.Remove(flight);
        await context.SaveChangesAsync(cancellationToken);

        return flight.Id;
    }
}