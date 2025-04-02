using Domain.Users;
using MediatR;

namespace Application.Users.Login;

internal sealed class UserLoggedInDomainEventHandler : INotificationHandler<UserLoggedInDomainEvent>
{
    public Task Handle(UserLoggedInDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\n[EVENT] User with ID {notification.UserId} has been logged in. [{DateTime.UtcNow}]\n");
        // TODO: Send an email verification
        return Task.CompletedTask;
    }
}
