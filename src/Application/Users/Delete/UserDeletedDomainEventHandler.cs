using Domain.Users;
using MediatR;

namespace Application.Users.Delete;

internal sealed class UserDeletedDomainEventHandler : INotificationHandler<UserDeletedDomainEvent>
{
    public Task Handle(UserDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\nUser with ID {notification.UserId} has been deleted.\n");

        return Task.CompletedTask;
    }
}
