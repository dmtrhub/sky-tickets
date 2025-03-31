using Domain.Flights;
using MediatR;

namespace Application.Flights.Delete;

internal sealed class FlightDeletedDomainEventHandler : INotificationHandler<FlightDeletedDomainEvent>
{
    public Task Handle(FlightDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\n[EVENT] Flight with ID {notification.Id}: {notification.Departure.ToUpper()}" +
            $" -> {notification.Destination.ToUpper()} deleted. [{DateTime.UtcNow}]\n");

        return Task.CompletedTask;
    }
}
