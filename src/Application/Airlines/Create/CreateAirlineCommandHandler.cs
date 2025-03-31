using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain;
using Domain.Airlines;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Airlines.Create;

public sealed class CreateAirlineCommandHandler(IApplicationDbContext context) 
    : ICommandHandler<CreateAirlineCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateAirlineCommand command, CancellationToken cancellationToken)
    {
        if (await context.Airlines.AnyAsync(a => a.Name == command.Name, cancellationToken))
            return Result.Failure<Guid>(AirlineErrors.NameInUse(command.Name));

        var airline = command.ToAirline();

        airline.Raise(new AirlineCreatedDomainEvent(airline.Name));

        await context.Airlines.AddAsync(airline, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return airline.Id;
    }
}