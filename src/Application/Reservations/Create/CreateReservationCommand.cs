using Application.Abstractions.Messaging;

namespace Application.Reservations.Create;

public sealed record CreateReservationCommand(Guid FlightId, int PassengerCount) : ICommand<Guid>;
