using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Airlines;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Airlines.GetAll;

public sealed class GetAllAirlinesQueryHandler(IRepository<Airline> airlineRepository)
    : IQueryHandler<GetAllAirlinesQuery, List<AirlineResponse>>
{
    public async Task<Result<List<AirlineResponse>>> Handle(GetAllAirlinesQuery query, CancellationToken cancellationToken)
    {
        var airlineQueryable = await airlineRepository.AsQueryable();

        var airlines = await airlineQueryable
            .Include(a => a.Flights)
            .Include(a => a.Reviews)
            .Select(a => a.ToAirlineResponse())
            .ToListAsync(cancellationToken);

        if (airlines.Count == 0)
            return Result.Failure<List<AirlineResponse>>(AirlineErrors.NoAirlinesFound);

        return airlines;
    }
}