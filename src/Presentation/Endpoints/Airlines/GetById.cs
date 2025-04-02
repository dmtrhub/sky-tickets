using Application.Airlines;
using Application.Airlines.GetById;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Airlines;

public sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/airlines/{id}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetAirlineByIdQuery(id);

            Result<AirlineResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Airlines)
        .RequireAuthorization(AuthorizationPolicies.AdministratorPolicy)
        .RequireAuthorization(AuthorizationPolicies.PassengerPolicy);
    }
}
