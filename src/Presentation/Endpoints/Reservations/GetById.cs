using Application.Reservations;
using Application.Reservations.GetById;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Reservations;

public class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/reservations/{id}", async (Guid id ,ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetReservationByIdQuery(id);

            Result<ReservationResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Reservations)
        .RequireAuthorization(AuthorizationPolicies.AdministratorPolicy);
    }
}
