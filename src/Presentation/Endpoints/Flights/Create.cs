using Application.Flights.Create;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Flights;

public sealed class Create : IEndpoint
{
    public sealed record CreateFlightRequest(
        Guid AirlineId,
        string Departure,
        string Destination,
        string DepartureTime,
        string ArrivalTime,
        int AvailableSeats,
        decimal Price);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/flights", async (CreateFlightRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateFlightCommand(
                request.AirlineId,
                request.Departure,
                request.Destination,
                request.DepartureTime,
                request.ArrivalTime,
                request.AvailableSeats,
                request.Price);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Created, CustomResults.Problem);
        })
        .WithTags(Tags.Flights);
    }
}
