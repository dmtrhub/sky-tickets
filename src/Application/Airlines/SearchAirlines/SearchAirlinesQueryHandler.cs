using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Airlines;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Airlines.SearchAirlines;

public sealed class SearchAirlinesQueryHandler(IRepository<Airline> airlineRepository) 
    : IQueryHandler<SearchAirlinesQuery, List<AirlineResponse>>
{
    public async Task<Result<List<AirlineResponse>>> Handle(SearchAirlinesQuery query, CancellationToken cancellationToken)
    {
        var airlinesQuery = await airlineRepository.AsQueryable();
        airlinesQuery.SearchAirlines(query);

        var airlines = await airlinesQuery
            .Select(a => a.ToAirlineResponse())
            .ToListAsync(cancellationToken);

        if (airlines.Count == 0)
            return Result.Failure<List<AirlineResponse>>(AirlineErrors.NoAirlinesFound);

        return airlines;
    }
}
