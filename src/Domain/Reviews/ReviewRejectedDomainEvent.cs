using SharedKernel;

namespace Domain.Reviews;

public sealed record ReviewRejectedDomainEvent(Guid Id) : IDomainEvent;
