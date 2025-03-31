using Application.Abstractions.Messaging;

namespace Application.Reservations.Delete;

public sealed record DeleteReservationCommand(Guid Id) : ICommand<Guid>;
