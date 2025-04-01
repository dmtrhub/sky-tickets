using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain;
using Domain.Reviews;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Security.Claims;

namespace Application.Reviews.Create;

public sealed class CreateReviewCommandHandler(
    IApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor)
    : ICommandHandler<CreateReviewCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateReviewCommand command, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            return Result.Failure<Guid>(UserErrors.Unauthenticated);

        var user = await context.Users.FindAsync(parsedUserId, cancellationToken);

        if (user is null)
            return Result.Failure<Guid>(UserErrors.NotFound(parsedUserId));

        bool hasCompletedFlight = await context.Reservations
            .Include(r => r.Flight)
            .ThenInclude(f => f.Airline)
            .AnyAsync(r => r.UserId == parsedUserId
                           && r.Flight.AirlineId == command.AirlineId
                           && r.Flight.Status == FlightStatus.Completed,
                      cancellationToken);

        if (!hasCompletedFlight)
            return Result.Failure<Guid>(ReviewErrors.NoCompletedFlightForAirline(command.AirlineId));

        bool reviewExists = await context.Reviews
            .AnyAsync(r => r.UserId == parsedUserId && r.AirlineId == command.AirlineId, cancellationToken);

        if (reviewExists)
            return Result.Failure<Guid>(ReviewErrors.AlreadyReviewed(command.AirlineId));

        var review = command.ToReview(parsedUserId);

        //event

        await context.Reviews.AddAsync(review, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return review.Id;
    }
}
