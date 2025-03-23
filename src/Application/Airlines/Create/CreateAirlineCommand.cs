using Application.Abstractions.Messaging;

namespace Application.Airlines.Create;

public sealed record CreateAirlineCommand(
    string Name,
    string Address,
    string ContactInfo) : ICommand<Guid>;