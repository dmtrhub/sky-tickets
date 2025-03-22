using Application.Users.Delete;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;
using Infrastructure.Authorization;

namespace Presentation.Endpoints.Users;

public sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/users/{id}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new DeleteUserCommand(id);

            Result<Guid> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization(AuthorizationPolicies.PassengerPolicy)
        .WithTags(Tags.Users);
    }
}
