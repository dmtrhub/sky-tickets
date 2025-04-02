using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Airlines;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Airlines.SearchAirlines;

public sealed class SearchAirlinesQueryHandler(IApplicationDbContext context) 
    : IQueryHandler<SearchAirlinesQuery, List<AirlineResponse>>
{
    public async Task<Result<List<AirlineResponse>>> Handle(SearchAirlinesQuery query, CancellationToken cancellationToken)
    {
        var airlinesQuery = context.Airlines.AsQueryable();

        airlinesQuery.SearchAirlines(query);

        var airlines = await airlinesQuery
            .Select(a => a.ToAirlineResponse())
            .ToListAsync(cancellationToken);

        if (airlines is null)
            return Result.Failure<List<AirlineResponse>>(AirlineErrors.NoAirlinesFound);

        return airlines;
    }
}
