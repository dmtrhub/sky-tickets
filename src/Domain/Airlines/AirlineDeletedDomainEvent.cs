using SharedKernel;

namespace Domain.Airlines;

public sealed record AirlineDeletedDomainEvent(string Name) : IDomainEvent;
