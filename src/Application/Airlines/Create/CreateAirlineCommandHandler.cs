using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain;
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

        var airline = new Airline
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Address = command.Address,
            ContactInfo = command.ContactInfo,
            Flights = [],
            Reviews = []
        };

        context.Airlines.Add(airline);
        await context.SaveChangesAsync(cancellationToken);

        return airline.Id;
    }
}