using Application.Abstractions.Data;
using Domain;
using Domain.Flights;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Flights;

public class FlightStatusUpdater(IApplicationDbContext context)
{
    public async Task UpdateFlightStatuses()
    {
        var now = DateTime.UtcNow;
        var flightsToComplete = await context.Flights
            .Where(f => f.Status == FlightStatus.Active && f.ArrivalTime <= now)
            .ToListAsync();

        foreach (var flight in flightsToComplete)
        {
            flight.Status = FlightStatus.Completed;
            flight.Raise(new FlightCompletedDomainEvent(flight.Id, flight.Departure, flight.Destination));
        }

        await context.SaveChangesAsync();
    }
}

internal sealed class FlightCompletedDomainEventHandler : INotificationHandler<FlightCompletedDomainEvent>
{
    public Task Handle(FlightCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\n[EVENT] Flight with ID {notification.Id}: {notification.Departure.ToUpper()}" +
            $" -> {notification.Destination.ToUpper()} completed. [{DateTime.UtcNow}]\n");

        return Task.CompletedTask;
    }
}
