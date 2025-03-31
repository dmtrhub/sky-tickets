using Application.Abstractions.Messaging;

namespace Application.Reservations.GetAll;

public sealed record GetAllReservationsQuery() : IQuery<List<ReservationResponse>>;
