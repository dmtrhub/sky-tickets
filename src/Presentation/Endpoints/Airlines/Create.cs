using Application.Airlines.Create;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel;

namespace Presentation.Endpoints.Airlines;

public class Create : IEndpoint
{
    public sealed record CreateAirlineRequest(
        string Name,
        string Address,
        string ContactInfo);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/airlines", async (CreateAirlineRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateAirlineCommand(request.Name, request.Address, request.ContactInfo);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Created, CustomResults.Problem);
        })
        .WithTags(Tags.Airlines);
    }
}
