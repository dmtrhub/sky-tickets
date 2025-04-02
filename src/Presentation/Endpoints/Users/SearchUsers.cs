using Application.Users;
using Application.Users.SearchUsers;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Users;

public sealed class SearchUsers : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/search", async (
            string? firstName,
            string? lastName,
            DateOnly? dateOfBirthFrom,
            DateOnly? dateOfBirthTo, 
            ISender sender, 
            CancellationToken cancellationToken) =>
        {
            var query = new SearchUsersQuery(firstName, lastName, dateOfBirthFrom, dateOfBirthTo);

            Result<List<UserResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Users)
        .RequireAuthorization(AuthorizationPolicies.AdministratorPolicy);
    }
}