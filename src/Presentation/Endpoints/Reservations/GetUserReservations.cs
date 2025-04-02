using Application.Reservations;
using Application.Reservations.GetUserReservations;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Reservations;

public class GetUserReservations : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("reservations/user", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetUserReservationsQuery();

            Result<List<ReservationResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Reservations)
        .RequireAuthorization(AuthorizationPolicies.PassengerPolicy);
    }
}
