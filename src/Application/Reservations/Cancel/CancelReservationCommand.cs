using Application.Abstractions.Messaging;

namespace Application.Reservations.Cancel;

public sealed record CancelReservationCommand(Guid Id) : ICommand<Guid>;
