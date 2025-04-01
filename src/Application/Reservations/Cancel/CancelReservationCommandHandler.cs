using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain;
using Domain.Reservations;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Security.Claims;

namespace Application.Reservations.Cancel;

public sealed class CancelReservationCommandHandler(
    IApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor)
    : ICommandHandler<CancelReservationCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CancelReservationCommand command, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            return Result.Failure<Guid>(UserErrors.Unauthenticated);

        var reservation = await context.Reservations
            .Where(r => r.Id == command.Id && r.UserId == parsedUserId &&
                (r.Status == ReservationStatus.Created || r.Status == ReservationStatus.Approved))
            .Include(r => r.Flight)
            .FirstOrDefaultAsync(cancellationToken);

        if (reservation is null)
            return Result.Failure<Guid>(ReservationErrors.NotFound(command.Id));

        if (reservation.Flight.DepartureTime < DateTime.UtcNow.AddHours(24))
            return Result.Failure<Guid>(ReservationErrors.CannotCancel);

        reservation.CancelReservation();

        await context.SaveChangesAsync(cancellationToken);

        return reservation.Id;
    }
}
