using Application.Flights;
using Application.Flights.GetAll;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Flights;

public sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/flights", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetAllFlightsQuery();

            Result<List<FlightResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Flights);
    }
}
