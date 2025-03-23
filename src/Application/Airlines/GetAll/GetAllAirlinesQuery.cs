using Application.Abstractions.Messaging;

namespace Application.Airlines.GetAll;

public sealed record GetAllAirlinesQuery() : IQuery<List<AirlineResponse>>;