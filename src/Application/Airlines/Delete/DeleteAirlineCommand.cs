using Application.Abstractions.Messaging;

namespace Application.Airlines.Delete;

public sealed record DeleteAirlineCommand(Guid Id) : ICommand<Guid>;