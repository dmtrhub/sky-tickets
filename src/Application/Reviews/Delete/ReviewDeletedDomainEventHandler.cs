using Domain.Reviews;
using MediatR;

namespace Application.Reviews.Delete;

internal sealed class ReviewDeletedDomainEventHandler : INotificationHandler<ReviewDeletedDomainEvent>
{
    public Task Handle(ReviewDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\n[EVENT] Review with ID {notification.Id} deleted. [{DateTime.UtcNow}]\n");

        return Task.CompletedTask;
    }
}
