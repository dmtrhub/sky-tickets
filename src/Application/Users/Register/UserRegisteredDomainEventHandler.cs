using Domain.Users;
using MediatR;

namespace Application.Users.Register;

internal sealed class UserRegisteredDomainEventHandler : INotificationHandler<UserRegisteredDomainEvent>
{
    public Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\n[EVENT] User with ID {notification.UserId} has been registered. [{DateTime.UtcNow}]\n");
        // TODO: Send an email verification
        return Task.CompletedTask;
    }
}
