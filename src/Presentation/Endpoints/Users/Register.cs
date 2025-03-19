using Application.Users.Register;
using Domain;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;
using System.Text.Json.Serialization;

namespace Presentation.Endpoints.Users;

internal sealed class Register : IEndpoint
{
    public sealed record RegisterRequest(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string DateOfBirth,
        string Gender);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register", async (RegisterRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new RegisterUserCommand(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                request.DateOfBirth,
                request.Gender);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Users);
    }
}
