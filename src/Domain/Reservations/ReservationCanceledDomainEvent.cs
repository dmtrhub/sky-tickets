using SharedKernel;

namespace Domain.Reservations;

public sealed record ReservationCanceledDomainEvent(Guid Id) : IDomainEvent;
