using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Extensions;
using Domain;
using Domain.Reservations;
using Domain.Reviews;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reviews.Create;

public sealed class CreateReviewCommandHandler(
    IRepository<Review> reviewRepository,
    IRepository<User> userRepository,
    IRepository<Reservation> reservationRepository,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor) : ICommandHandler<CreateReviewCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateReviewCommand command, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.GetUserId();

        if (userId is null)
            return Result.Failure<Guid>(UserErrors.Unauthenticated);

        var user = await userRepository.GetByIdAsync(userId.Value, cancellationToken);

        if (user is null)
            return Result.Failure<Guid>(UserErrors.NotFound(userId.Value));

        var reservationQuery = await reservationRepository.AsQueryable();
        bool hasCompletedFlight = await reservationQuery
            .Include(r => r.Flight)
            .ThenInclude(f => f.Airline)
            .AnyAsync(r => r.UserId == userId
                        && r.Flight.AirlineId == command.AirlineId
                        && r.Flight.Status == FlightStatus.Completed,
                        cancellationToken);

        if (!hasCompletedFlight)
            return Result.Failure<Guid>(ReviewErrors.NoCompletedFlightForAirline(command.AirlineId));

        bool reviewExists = await reviewRepository
            .AnyAsync(r => r.UserId == userId && r.AirlineId == command.AirlineId, cancellationToken);

        if (reviewExists)
            return Result.Failure<Guid>(ReviewErrors.AlreadyReviewed(command.AirlineId));

        var review = command.ToReview(userId.Value);
        review.Raise(new ReviewCreatedDomainEvent(review.Id));

        await reviewRepository.AddAsync(review, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return review.Id;
    }
}
