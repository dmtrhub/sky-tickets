using Domain.Reviews;
using MediatR;

namespace Application.Reviews.Approve;

internal sealed class ReviewApprovedDomainEventHandler : INotificationHandler<ReviewApprovedDomainEvent>
{
    public Task Handle(ReviewApprovedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\n[EVENT] Review with ID {notification.Id} approved. [{DateTime.UtcNow}]\n");

        return Task.CompletedTask;
    }
}

internal sealed class ReviewRejectedDomainEventHandler : INotificationHandler<ReviewRejectedDomainEvent>
{
    public Task Handle(ReviewRejectedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\n[EVENT] Review with ID {notification.Id} rejected. [{DateTime.UtcNow}]\n");

        return Task.CompletedTask;
    }
}
