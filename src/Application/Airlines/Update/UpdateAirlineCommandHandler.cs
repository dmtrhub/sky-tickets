using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Airlines;
using SharedKernel;

namespace Application.Airlines.Update;

public sealed class UpdateAirlineCommandHandler(
    IRepository<Airline> airlineRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateAirlineCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateAirlineCommand command, CancellationToken cancellationToken)
    {
        var airline = await airlineRepository.GetByIdAsync(command.Id, cancellationToken);

        if (airline is null)
            return Result.Failure<Guid>(AirlineErrors.NotFound(command.Id));

        if (await airlineRepository.AnyAsync(a => a.Name == command.Name && a.Id != command.Id, cancellationToken))
            return Result.Failure<Guid>(AirlineErrors.NameInUse(command.Name!));

        airline.UpdateAirline(command);
        airline.Raise(new AirlineUpdatedDomainEvent(airline.Name));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return airline.Id;
    }
}