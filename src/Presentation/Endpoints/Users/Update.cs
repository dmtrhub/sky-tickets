using Application.Users.Update;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Users;

public sealed class Update : IEndpoint
{
    public sealed record UpdateRequest(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string DateOfBirth,
        string Gender,
        string Role);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/users/{id}", async (Guid id, UpdateRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UpdateUserCommand(
                id,
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                request.DateOfBirth,
                request.Gender,
                request.Role);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Users);
    }
}