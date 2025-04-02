using SharedKernel;

namespace Domain.Users;

public sealed record UserLoggedInDomainEvent(Guid UserId) : IDomainEvent;