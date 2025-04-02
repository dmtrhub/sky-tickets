using Application.Flights;
using Application.Flights.GetById;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Flights;

public sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/flights/{id}", async (Guid id, ISender sender, CancellationToken cancellation) =>
        {
            var query = new GetFlightByIdQuery(id);

            Result<FlightResponse> result = await sender.Send(query, cancellation);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Flights)
        .RequireAuthorization(AuthorizationPolicies.AdministratorPolicy);
    }
}
