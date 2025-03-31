using Domain.Airlines;
using MediatR;

namespace Application.Airlines.Delete;

internal sealed class AirlineDeletedDomainEventHandler : INotificationHandler<AirlineDeletedDomainEvent>
{
    public Task Handle(AirlineDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\n[EVENT] Airline '{notification.Name}' deleted. [{DateTime.UtcNow}]\n");

        return Task.CompletedTask;
    }
}
