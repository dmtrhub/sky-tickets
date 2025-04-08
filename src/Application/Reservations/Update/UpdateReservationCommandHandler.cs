using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Reservations;
using SharedKernel;

namespace Application.Reservations.Update;

public sealed class UpdateReservationCommandHandler(
    IRepository<Reservation> reservationRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateReservationCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateReservationCommand command, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.GetByIdAsync(command.Id, cancellationToken);

        if (reservation is null)
            return Result.Failure<Guid>(ReservationErrors.NotFound(command.Id));

        reservation.UpdateReservation(command);
        reservation.Raise(new ReservationUpdatedDomainEvent(reservation.Id));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return reservation.Id;
    }
}
