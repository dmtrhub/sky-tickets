using Application.Reviews;
using Application.Reviews.GetUserReviews;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Reviews;

public class GetUserReviews : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/reviews/user", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetUserReviewsQuery();

            Result<List<ReviewResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Reviews)
        .RequireAuthorization(AuthorizationPolicies.PassengerPolicy);
    }
}
