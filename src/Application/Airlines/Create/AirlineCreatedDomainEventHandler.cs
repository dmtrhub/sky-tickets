using Domain.Airlines;
using MediatR;

namespace Application.Airlines.Create;

public sealed class AirlineCreatedDomainEventHandler : INotificationHandler<AirlineCreatedDomainEvent>
{
    public Task Handle(AirlineCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\n[EVENT] Airline '{notification.Name}' created. [{DateTime.UtcNow}]\n");

        return Task.CompletedTask;
    }
}
