using Application.Reviews.Create;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Reviews;

public class Create : IEndpoint
{
    public sealed record CreateReviewRequest(
        Guid AirlineId,
        string Title,
        string Content,
        string? ImageUrl);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/reviews", async (CreateReviewRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateReviewCommand(request.AirlineId, request.Title, request.Content, request.ImageUrl);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Reviews)
        .RequireAuthorization(AuthorizationPolicies.PassengerPolicy);
    }
}
