using Domain.Airlines;
using MediatR;

namespace Application.Airlines.Update;

internal sealed class AirlineUpdatedDomainEventHandler : INotificationHandler<AirlineUpdatedDomainEvent>
{
    public Task Handle(AirlineUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\n[EVENT] Airline '{notification.Name}' updated. [{DateTime.UtcNow}]\n");

        return Task.CompletedTask;
    }
}
