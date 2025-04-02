using Application.Reviews;
using Application.Reviews.GetAll;
using Application.Reviews.GetById;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Reviews;

public class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/reviews/{id}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetReviewByIdQuery(id);

            Result<ReviewResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Reviews)
        .RequireAuthorization(AuthorizationPolicies.AdministratorPolicy);
    }
}