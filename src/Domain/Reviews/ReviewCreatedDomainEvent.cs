using SharedKernel;

namespace Domain.Reviews;

public sealed record ReviewCreatedDomainEvent(Guid Id) : IDomainEvent;
