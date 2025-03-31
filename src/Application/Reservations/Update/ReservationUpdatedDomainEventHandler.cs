using Domain.Reservations;
using MediatR;

namespace Application.Reservations.Update;

internal sealed class ReservationUpdatedDomainEventHandler : INotificationHandler<ReservationUpdatedDomainEvent>
{
    public Task Handle(ReservationUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\n[EVENT] Reservation with ID {notification.Id} updated. [{DateTime.UtcNow}]\n");

        return Task.CompletedTask;
    }
}
