using Application.Flights;
using Application.Flights.GetUserFlights;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Flights;

public class GetUserFlights : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("flights/user", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetUserFlightsQuery();

            Result<List<FlightResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Flights)
        .RequireAuthorization(AuthorizationPolicies.PassengerPolicy);
    }
}
