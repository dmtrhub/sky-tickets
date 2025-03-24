using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain;
using SharedKernel;

namespace Application.Airlines.Delete;

public sealed class DeleteAirlineCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteAirlineCommand, Guid>
{
    public async Task<Result<Guid>> Handle(DeleteAirlineCommand command, CancellationToken cancellationToken)
    {
        var airline = await context.Airlines.FindAsync([command.Id], cancellationToken);

        if (airline is null)
            return Result.Failure<Guid>(AirlineErrors.NotFound(command.Id));

        context.Airlines.Remove(airline);
        await context.SaveChangesAsync(cancellationToken);

        return airline.Id;
    }
}