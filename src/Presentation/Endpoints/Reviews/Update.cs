using Application.Reviews.Update;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Reviews;

public class Update : IEndpoint
{
    public sealed record UpdateReviewRequest(
        string Title,
        string Content,
        string? ImageUrl);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/reviews/{id}", async (Guid id, UpdateReviewRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UpdateReviewCommand(id, request.Title, request.Content, request.ImageUrl);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Reviews);
    }
}
