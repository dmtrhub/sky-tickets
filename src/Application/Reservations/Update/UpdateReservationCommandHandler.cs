using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Reservations;
using SharedKernel;

namespace Application.Reservations.Update;

public sealed class UpdateReservationCommandHandler(IApplicationDbContext context)
    : ICommandHandler<UpdateReservationCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateReservationCommand command, CancellationToken cancellationToken)
    {
        var reservation = await context.Reservations.FindAsync(command.Id, cancellationToken);

        if (reservation is null)
            return Result.Failure<Guid>(ReservationErrors.NotFound(command.Id));

        reservation.UpdateReservation(command);

        reservation.Raise(new ReservationUpdatedDomainEvent(reservation.Id));

        await context.SaveChangesAsync(cancellationToken);

        return reservation.Id;
    }
}
