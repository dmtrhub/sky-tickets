using SharedKernel;

namespace Domain.Reservations;

public sealed record ReservationCreatedDomainEvent(Guid Id) : IDomainEvent;
