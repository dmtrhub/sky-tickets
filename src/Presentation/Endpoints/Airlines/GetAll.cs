using Application.Airlines;
using Application.Airlines.GetAll;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Airlines;

public sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/airlines", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetAllAirlinesQuery();

            Result<List<AirlineResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Airlines);
    }
}
