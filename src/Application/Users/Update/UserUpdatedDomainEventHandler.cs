using Domain.Users;
using MediatR;

namespace Application.Users.Update;

internal sealed class UserUpdatedDomainEventHandler : INotificationHandler<UserUpdatedDomainEvent>
{
    public Task Handle(UserUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\n[EVENT] User with ID {notification.UserId} has been updated. [{DateTime.UtcNow}]\n");

        return Task.CompletedTask;
    }
}
