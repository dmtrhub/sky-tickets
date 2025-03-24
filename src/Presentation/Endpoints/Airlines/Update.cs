using Application.Airlines.Update;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Airlines;

public sealed class Update : IEndpoint
{
    public sealed record UpdateAirlineRequest(
        string Name,
        string Address,
        string ContactInfo);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/airlines/{id}", async (Guid id, UpdateAirlineRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UpdateAirlineCommand(id, request.Name, request.Address, request.ContactInfo);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Airlines)
        .RequireAuthorization(AuthorizationPolicies.AdministratorPolicy);
    }
}
