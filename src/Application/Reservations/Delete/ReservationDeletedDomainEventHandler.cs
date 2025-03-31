using Domain.Reservations;
using MediatR;

namespace Application.Reservations.Delete;

internal sealed class ReservationDeletedDomainEventHandler : INotificationHandler<ReservationDeletedDomainEvent>
{
    public Task Handle(ReservationDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\n[EVENT] Reservation with ID {notification.Id} deleted. [{DateTime.UtcNow}]\n");

        return Task.CompletedTask;
    }
}
