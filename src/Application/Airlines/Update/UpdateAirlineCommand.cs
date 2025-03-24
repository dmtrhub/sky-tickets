using Application.Abstractions.Messaging;

namespace Application.Airlines.Update;

public sealed record UpdateAirlineCommand(
    Guid Id,
    string Name,
    string Address,
    string ContactInfo) : ICommand<Guid>;