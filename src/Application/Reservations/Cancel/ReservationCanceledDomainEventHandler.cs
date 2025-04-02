using Domain.Reservations;
using MediatR;

namespace Application.Reservations.Cancel;

internal sealed class ReservationCanceledDomainEventHandler : INotificationHandler<ReservationCanceledDomainEvent>
{
    public Task Handle(ReservationCanceledDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"\n[EVENT] Reservation with ID {notification.Id} canceled. [{DateTime.UtcNow}]\n");

        return Task.CompletedTask;
    }
}
