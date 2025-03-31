using Application.Abstractions.Messaging;

namespace Application.Reservations.Update;

public sealed record UpdateReservationCommand(
    Guid Id,
    int? PassengerCount,
    string? Status) : ICommand<Guid>;
