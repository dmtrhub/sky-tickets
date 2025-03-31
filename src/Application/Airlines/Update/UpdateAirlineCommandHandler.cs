using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain;
using Domain.Airlines;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Airlines.Update;

public sealed class UpdateAirlineCommandHandler(IApplicationDbContext context)
    : ICommandHandler<UpdateAirlineCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateAirlineCommand command, CancellationToken cancellationToken)
    {
        var airline = await context.Airlines.FindAsync([command.Id], cancellationToken);

        if (airline is null)
            return Result.Failure<Guid>(AirlineErrors.NotFound(command.Id));

        if (await context.Airlines.AnyAsync(a => a.Name == command.Name && a.Id != command.Id, cancellationToken))
            return Result.Failure<Guid>(AirlineErrors.NameInUse(command.Name!));

        airline.UpdateAirline(command);

        airline.Raise(new AirlineUpdatedDomainEvent(airline.Name));

        await context.SaveChangesAsync(cancellationToken);

        return airline.Id;
    }
}