using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Extensions;
using Domain;
using Domain.Reviews;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reviews.Create;

public sealed class CreateReviewCommandHandler(
    IApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor) : ICommandHandler<CreateReviewCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateReviewCommand command, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.GetUserId();

        if (userId is null)
            return Result.Failure<Guid>(UserErrors.Unauthenticated);

        var user = await context.Users.FindAsync(userId, cancellationToken);

        if (user is null)
            return Result.Failure<Guid>(UserErrors.NotFound(userId.Value));

        bool hasCompletedFlight = await context.Reservations
            .Include(r => r.Flight)
            .ThenInclude(f => f.Airline)
            .AnyAsync(r => r.UserId == userId
                        && r.Flight.AirlineId == command.AirlineId
                        && r.Flight.Status == FlightStatus.Completed,
                        cancellationToken);

        if (!hasCompletedFlight)
            return Result.Failure<Guid>(ReviewErrors.NoCompletedFlightForAirline(command.AirlineId));

        bool reviewExists = await context.Reviews
            .AnyAsync(r => r.UserId == userId && r.AirlineId == command.AirlineId, cancellationToken);

        if (reviewExists)
            return Result.Failure<Guid>(ReviewErrors.AlreadyReviewed(command.AirlineId));

        var review = command.ToReview(userId.Value);

        review.Raise(new ReviewCreatedDomainEvent(review.Id));

        await context.Reviews.AddAsync(review, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return review.Id;
    }
}
