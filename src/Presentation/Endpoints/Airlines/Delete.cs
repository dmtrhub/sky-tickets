using Application.Airlines.Delete;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Airlines;

public sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/airlines/{id}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new DeleteAirlineCommand(id);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Airlines)
        .RequireAuthorization(AuthorizationPolicies.AdministratorPolicy);
    }
}
