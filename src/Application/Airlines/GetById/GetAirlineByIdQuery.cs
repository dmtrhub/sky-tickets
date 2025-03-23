using Application.Abstractions.Messaging;

namespace Application.Airlines.GetById;

public sealed record GetAirlineByIdQuery(Guid Id) : IQuery<AirlineResponse>;