using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Airlines.GetById;

public sealed class GetAirlineByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetAirlineByIdQuery, AirlineResponse>
{
    public async Task<Result<AirlineResponse>> Handle(GetAirlineByIdQuery query, CancellationToken cancellationToken)
    {
        var airline = await context.Airlines
            .Where(a => a.Id == query.Id)
            .Select(a => new AirlineResponse
            {
                Id = a.Id,
                Name = a.Name,
                Address = a.Address,
                ContactInfo = a.ContactInfo
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (airline is null)
            return Result.Failure<AirlineResponse>(AirlineErrors.NotFound(query.Id));

        return airline;
    }
}