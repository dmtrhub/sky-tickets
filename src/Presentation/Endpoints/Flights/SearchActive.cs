using Application.Flights;
using Application.Flights.SearchActive;
using Infrastructure.Authorization;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Flights;

public class SearchActive : IEndpoint
{ 
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
            app.MapGet("/flights/search", 
                async (
                    string? destination,
                    DateTime? date,
                    int? minSeatsAvailable,
                    decimal? maxPrice, 
                    ISender sender, 
                    CancellationToken cancellationToken) =>
            {
                    var query = new SearchActiveFlightsQuery(destination, date, minSeatsAvailable, maxPrice);

                    Result<List<FlightResponse>> result = await sender.Send(query, cancellationToken);

                    return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Flights)
            .RequireAuthorization(AuthorizationPolicies.AdministratorPolicy)
            .RequireAuthorization(AuthorizationPolicies.PassengerPolicy);
    }
}
