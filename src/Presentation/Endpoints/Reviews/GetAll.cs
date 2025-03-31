using Application.Reviews;
using Application.Reviews.GetAll;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Reviews;

public class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/reviews", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetAllReviewsQuery();

            Result<List<ReviewResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Reviews);
    }
}
