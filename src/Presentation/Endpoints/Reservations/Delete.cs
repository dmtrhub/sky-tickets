using Application.Reservations.Delete;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Reservations;

public class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/reservations/{id}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new DeleteReservationCommand(id);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Reservations)
        .RequireAuthorization(AuthorizationPolicies.AdministratorPolicy);
    }
}
