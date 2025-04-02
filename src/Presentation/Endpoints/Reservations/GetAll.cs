using Application.Reservations;
using Application.Reservations.GetAll;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Reservations;

public class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/reservations", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetAllReservationsQuery();

            Result<List<ReservationResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Reservations)
        .RequireAuthorization(AuthorizationPolicies.AdministratorPolicy);
    }
}
