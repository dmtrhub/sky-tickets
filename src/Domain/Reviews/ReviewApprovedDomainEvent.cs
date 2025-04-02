using SharedKernel;

namespace Domain.Reviews;

public sealed record ReviewApprovedDomainEvent(Guid Id) : IDomainEvent;
