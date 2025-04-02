using Application.Reviews.Approve;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Reviews;

public class Approve : IEndpoint
{
    public sealed record ApproveReviewRequest(bool IsApproved);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/reviews/approve/{id}", async (Guid id, ApproveReviewRequest request, ISender sender, CancellationToken cancellation) =>
        {
            var command = new ApproveReviewCommand(id, request.IsApproved);

            Result<Guid> result = await sender.Send(command, cancellation);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Reviews)
        .RequireAuthorization(AuthorizationPolicies.AdministratorPolicy);
    }
}
