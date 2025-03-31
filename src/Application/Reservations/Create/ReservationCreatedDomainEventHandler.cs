using Domain.Reservations;
using MediatR;

namespace Application.Reservations.Create;

internal sealed class ReservationCreatedDomainEventHandler : INotificationHandler<ReservationCreatedDomainEvent>
{
    public Task Handle(ReservationCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\n[EVENT] Reservation with ID {notification.Id} created. [{DateTime.UtcNow}]\n");

        return Task.CompletedTask;
    }
}
