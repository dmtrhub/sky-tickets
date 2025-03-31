using Domain.Flights;
using MediatR;

namespace Application.Flights.Update;

internal sealed class FlightUpdatedDomainEventHandler : INotificationHandler<FlightUpdatedDomainEvent>
{
    public Task Handle(FlightUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\n[EVENT] Flight with ID {notification.Id}: {notification.Departure.ToUpper()}" +
            $" -> {notification.Destination.ToUpper()} updated. [{DateTime.UtcNow}]\n");

        return Task.CompletedTask;
    }
}
