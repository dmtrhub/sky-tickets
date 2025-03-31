using Application.Reservations.Create;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Reservations;

public sealed class Create : IEndpoint
{
    public sealed record CreateReservationRequest(Guid FlightId, int PassengerCount);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/reservations", async (CreateReservationRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateReservationCommand(request.FlightId, request.PassengerCount);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Created, CustomResults.Problem);
        })
        .WithTags(Tags.Reservations)
        .RequireAuthorization(AuthorizationPolicies.PassengerPolicy);
    }
}
