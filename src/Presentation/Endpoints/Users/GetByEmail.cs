using Application.Users;
using Application.Users.GetByEmail;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Users;

public sealed class GetByEmail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/by-email/{email}", async (string email, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetUserByEmailQuery(email);

            Result<UserResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Users)
        .RequireAuthorization(AuthorizationPolicies.AdministratorPolicy);
    }
}