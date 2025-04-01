using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Airlines.SearchAirlines;

public sealed class SearchAirlinesQueryHandler(
    IApplicationDbContext context) : IQueryHandler<SearchAirlinesQuery, List<AirlineResponse>>
{
    public async Task<Result<List<AirlineResponse>>> Handle(SearchAirlinesQuery query, CancellationToken cancellationToken)
    {
        var airlinesQuery = context.Airlines.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Name))
            airlinesQuery = airlinesQuery.Where(a => a.Name.Contains(query.Name));

        if (!string.IsNullOrWhiteSpace(query.Address))
            airlinesQuery = airlinesQuery.Where(a => a.Address.Contains(query.Address));

        if (!string.IsNullOrWhiteSpace(query.ContactInfo))
            airlinesQuery = airlinesQuery.Where(a => a.ContactInfo.Contains(query.ContactInfo));

        var airlines = await airlinesQuery
            .Select(a => a.ToAirlineResponse())
            .ToListAsync(cancellationToken);

        return airlines;
    }
}
