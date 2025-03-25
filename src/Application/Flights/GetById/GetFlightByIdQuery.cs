using Application.Abstractions.Messaging;

namespace Application.Flights.GetById;

public sealed record GetFlightByIdQuery(Guid Id) : IQuery<FlightResponse>;