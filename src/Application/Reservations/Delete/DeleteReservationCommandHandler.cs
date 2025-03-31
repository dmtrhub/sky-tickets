using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Reservations;
using SharedKernel;

namespace Application.Reservations.Delete;

public sealed class DeleteReservationCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteReservationCommand, Guid>
{
    public async Task<Result<Guid>> Handle(DeleteReservationCommand command, CancellationToken cancellationToken)
    {
        var reservation = await context.Reservations.FindAsync(command.Id, cancellationToken);

        if(reservation is null)
            return Result.Failure<Guid>(ReservationErrors.NotFound(command.Id));

        reservation.Raise(new ReservationDeletedDomainEvent(reservation.Id));

        context.Reservations.Remove(reservation);
        await context.SaveChangesAsync(cancellationToken);

        return reservation.Id;
    }
}