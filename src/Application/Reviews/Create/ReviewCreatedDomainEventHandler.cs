using Domain.Reviews;
using MediatR;

namespace Application.Reviews.Create;

internal sealed class ReviewCreateddDomainEventHandler : INotificationHandler<ReviewCreatedDomainEvent>
{
    public Task Handle(ReviewCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\n[EVENT] Review with ID {notification.Id} created. [{DateTime.UtcNow}]\n");

        return Task.CompletedTask;
    }
}
