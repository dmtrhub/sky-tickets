using Domain.Users;
using MediatR;

namespace Application.Users.Update;

internal sealed class UserUpdatedDomainEventHandler : INotificationHandler<UserUpdatedDomainEvent>
{
    public Task Handle(UserUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\nUser with ID {notification.UserId} has been updated.\n");

        return Task.CompletedTask;
    }
}
