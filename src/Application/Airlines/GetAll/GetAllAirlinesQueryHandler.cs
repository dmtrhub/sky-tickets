using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Airlines;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Airlines.GetAll;

public sealed class GetAllAirlinesQueryHandler(IApplicationDbContext context) 
    : IQueryHandler<GetAllAirlinesQuery, List<AirlineResponse>>
{
    public async Task<Result<List<AirlineResponse>>> Handle(GetAllAirlinesQuery query, CancellationToken cancellationToken)
    {
        var airlines = await context.Airlines
            .Include(a => a.Flights)
            .Include(a => a.Reviews)
            .Select(a => a.ToAirlineResponse())
            .ToListAsync(cancellationToken);

        if (airlines is null)
            return Result.Failure<List<AirlineResponse>>(AirlineErrors.NoAirlinesFound);

        return airlines;
    }
}