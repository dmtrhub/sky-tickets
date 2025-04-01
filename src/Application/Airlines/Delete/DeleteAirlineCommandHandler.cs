using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain;
using Domain.Airlines;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Airlines.Delete;

public sealed class DeleteAirlineCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteAirlineCommand, Guid>
{
    public async Task<Result<Guid>> Handle(DeleteAirlineCommand command, CancellationToken cancellationToken)
    {
        var airline = await context.Airlines
            .Include(a => a.Flights)
            .FirstOrDefaultAsync(a => a.Id == command.Id, cancellationToken);

        if (airline is null)
            return Result.Failure<Guid>(AirlineErrors.NotFound(command.Id));

        bool hasActiveFlights = airline.Flights.Any(f => f.Status == FlightStatus.Active);
        if (hasActiveFlights)
            return Result.Failure<Guid>(AirlineErrors.CannotDeleteWithActiveFlights);

        airline.Raise(new AirlineDeletedDomainEvent(airline.Name));

        context.Airlines.Remove(airline);
        await context.SaveChangesAsync(cancellationToken);

        return airline.Id;
    }
}