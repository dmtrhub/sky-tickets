using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Airlines;
using SharedKernel;

namespace Application.Airlines.Create;

public sealed class CreateAirlineCommandHandler(
    IRepository<Airline> airlineRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateAirlineCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateAirlineCommand command, CancellationToken cancellationToken)
    {
        if (await airlineRepository.AnyAsync(a => a.Name == command.Name, cancellationToken))
            return Result.Failure<Guid>(AirlineErrors.NameInUse(command.Name));

        var airline = command.ToAirline();
        airline.Raise(new AirlineCreatedDomainEvent(airline.Name));

        await airlineRepository.AddAsync(airline, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return airline.Id;
    }
}