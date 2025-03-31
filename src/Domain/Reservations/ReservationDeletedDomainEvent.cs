using SharedKernel;

namespace Domain.Reservations;

public sealed record ReservationDeletedDomainEvent(Guid Id) : IDomainEvent;