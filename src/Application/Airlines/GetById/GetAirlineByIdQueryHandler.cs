using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Airlines;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Airlines.GetById;

public sealed class GetAirlineByIdQueryHandler(IRepository<Airline> airlineRepository)
    : IQueryHandler<GetAirlineByIdQuery, AirlineResponse>
{
    public async Task<Result<AirlineResponse>> Handle(GetAirlineByIdQuery query, CancellationToken cancellationToken)
    {
        var airlineQueryable = await airlineRepository.AsQueryable();

        var airline = await airlineQueryable
            .Where(a => a.Id == query.Id)
            .Include(a => a.Flights)
            .Include(a => a.Reviews)
            .Select(a => a.ToAirlineResponse())
            .FirstOrDefaultAsync(cancellationToken);

        if (airline is null)
            return Result.Failure<AirlineResponse>(AirlineErrors.NotFound(query.Id));

        return airline;
    }
}