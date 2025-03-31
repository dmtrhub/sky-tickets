using Application.Abstractions.Messaging;

namespace Application.Reservations.GetById;

public sealed record GetReservationByIdQuery(Guid Id) : IQuery<ReservationResponse>;
