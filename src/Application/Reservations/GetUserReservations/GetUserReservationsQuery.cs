using Application.Abstractions.Messaging;

namespace Application.Reservations.GetUserReservations;

public sealed record GetUserReservationsQuery() : IQuery<List<ReservationResponse>>;
