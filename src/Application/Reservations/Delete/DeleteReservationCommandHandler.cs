using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Reservations;
using SharedKernel;

namespace Application.Reservations.Delete;

public sealed class DeleteReservationCommandHandler(
    IRepository<Reservation> reservationRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteReservationCommand, Guid>
{
    public async Task<Result<Guid>> Handle(DeleteReservationCommand command, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.GetByIdAsync(command.Id, cancellationToken);

        if(reservation is null)
            return Result.Failure<Guid>(ReservationErrors.NotFound(command.Id));

        reservation.Raise(new ReservationDeletedDomainEvent(reservation.Id));

        reservationRepository.Remove(reservation);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return reservation.Id;
    }
}