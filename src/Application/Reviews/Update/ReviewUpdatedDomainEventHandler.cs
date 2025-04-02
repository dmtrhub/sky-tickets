using Domain.Reviews;
using MediatR;

namespace Application.Reviews.Update;

internal sealed class ReviewUpdatedDomainEventHandler : INotificationHandler<ReviewUpdatedDomainEvent>
{
    public Task Handle(ReviewUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\n[EVENT] Review with ID {notification.Id} updated. [{DateTime.UtcNow}]\n");

        return Task.CompletedTask;
    }
}
