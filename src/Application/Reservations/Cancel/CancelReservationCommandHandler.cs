using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Extensions;
using Domain;
using Domain.Reservations;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reservations.Cancel;

public sealed class CancelReservationCommandHandler(
    IRepository<Reservation> reservationRepository,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor) : ICommandHandler<CancelReservationCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CancelReservationCommand command, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.GetUserId();
        if (userId is null)
            return Result.Failure<Guid>(UserErrors.Unauthenticated);

        var reservationQuery = await reservationRepository.AsQueryable();

        var reservation = await reservationQuery
            .Where(r => r.Id == command.Id && r.UserId == userId
                && (r.Status == ReservationStatus.Created
                || r.Status == ReservationStatus.Approved))
            .Include(r => r.Flight)
            .FirstOrDefaultAsync(cancellationToken);

        if (reservation is null)
            return Result.Failure<Guid>(ReservationErrors.NotFound(command.Id));

        if (reservation.Flight.DepartureTime < DateTime.UtcNow.AddHours(24))
            return Result.Failure<Guid>(ReservationErrors.CannotCancel);

        reservation.CancelReservation();
        reservation.Raise(new ReservationCanceledDomainEvent(reservation.Id));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return reservation.Id;
    }
}
