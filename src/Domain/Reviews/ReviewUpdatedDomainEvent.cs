using SharedKernel;

namespace Domain.Reviews;

public sealed record ReviewUpdatedDomainEvent(Guid Id) : IDomainEvent;
