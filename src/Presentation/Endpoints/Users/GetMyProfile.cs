using Application.Users;
using Application.Users.GetMyProfile;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Users;

public sealed class GetMyProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/my-profile", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetMyProfileQuery();

            Result<UserResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Users)
        .RequireAuthorization(AuthorizationPolicies.PassengerPolicy)
        .RequireAuthorization(AuthorizationPolicies.AdministratorPolicy);
    }
}