using Application.Airlines;
using Application.Airlines.SearchAirlines;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Airlines;

public sealed class SearchAirlines : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/airlines/search", async (string? name, string? address, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new SearchAirlinesQuery(name, address);

            Result<List<AirlineResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Airlines)
        .RequireAuthorization(AuthorizationPolicies.AdministratorPolicy)
        .RequireAuthorization(AuthorizationPolicies.PassengerPolicy);
    }
}
