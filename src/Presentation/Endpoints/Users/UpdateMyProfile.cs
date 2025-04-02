using Application.Users.UpdateMyProfile;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Users;

public sealed class UpdateMyProfile : IEndpoint
{
    public sealed record UpdateMyProfileRequest(
        string? FirstName,
        string? LastName,
        string? Email,
        string? Password,
        string? DateOfBirth,
        string? Gender);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/users/my-profile/update", async (UpdateMyProfileRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UpdateMyProfileCommand(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                request.DateOfBirth,
                request.Gender);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Users)
        .RequireAuthorization(AuthorizationPolicies.PassengerPolicy)
        .RequireAuthorization(AuthorizationPolicies.AdministratorPolicy);
    }
}