using Application.Reservations.Update;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Reservations;

public class Update : IEndpoint
{
    public sealed record UpdateReservationRequest(
        int? PassengerCount,
        string? Status);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/reservations/{id}", async (Guid id, UpdateReservationRequest request ,ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UpdateReservationCommand(id, request.PassengerCount, request.Status);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Reservations);
    }
}
