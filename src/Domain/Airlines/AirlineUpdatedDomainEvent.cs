using SharedKernel;

namespace Domain.Airlines;

public sealed record AirlineUpdatedDomainEvent(string Name) : IDomainEvent;
