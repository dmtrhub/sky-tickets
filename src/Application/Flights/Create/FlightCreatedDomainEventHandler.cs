using Domain.Flights;
using MediatR;

namespace Application.Flights.Create;

internal sealed class FlightCreatedDomainEventHandler : INotificationHandler<FlightCreatedDomainEvent>
{
    public Task Handle(FlightCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\n[EVENT] Flight with ID {notification.Id}: {notification.Departure.ToUpper()}" +
            $" -> {notification.Destination.ToUpper()} created. [{DateTime.UtcNow}]\n");

        return Task.CompletedTask;
    }
}
