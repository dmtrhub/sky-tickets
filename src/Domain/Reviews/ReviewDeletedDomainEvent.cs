using SharedKernel;

namespace Domain.Reviews;

public sealed record ReviewDeletedDomainEvent(Guid Id) : IDomainEvent;
