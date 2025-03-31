using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.AspNetCore.Http;
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

        var user = await context.Users.FindAsync([parsedUserId], cancellationToken);

        if (user is null)
            return Result.Failure<Guid>(UserErrors.NotFound(parsedUserId));

        var review = command.ToReview(parsedUserId);

        //event

        await context.Reviews.AddAsync(review, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return review.Id;
    }
}
