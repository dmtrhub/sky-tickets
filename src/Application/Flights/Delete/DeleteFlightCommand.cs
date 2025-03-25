using Application.Abstractions.Messaging;

namespace Application.Flights.Delete;

public sealed record DeleteFlightCommand(Guid Id) : ICommand<Guid>;