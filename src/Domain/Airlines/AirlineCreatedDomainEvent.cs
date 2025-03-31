using SharedKernel;

namespace Domain.Airlines;

public sealed record AirlineCreatedDomainEvent(string Name) : IDomainEvent;
