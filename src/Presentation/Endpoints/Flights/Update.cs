using Application.Flights.Update;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Flights;

public sealed class Update : IEndpoint
{
    public sealed record UpdateFlightRequest(
        string DepartureTime,
        string ArrivalTime,
        int AvailableSeats,
        int BookedSeats,
        decimal Price,
        string Status);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/flights/{id}", async (Guid id, UpdateFlightRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UpdateFlightCommand(
                id,
                request.DepartureTime,
                request.ArrivalTime,
                request.AvailableSeats,
                request.BookedSeats,
                request.Price,
                request.Status);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Flights)
        .RequireAuthorization(AuthorizationPolicies.AdministratorPolicy);
    }
}
