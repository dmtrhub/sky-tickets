using Application.Reviews;
using Application.Reviews.GetApproved;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Reviews;

public class GetApproved : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/reviews/approved", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetApprovedReviewsQuery();

            Result<List<ReviewResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Reviews);
    }
}
