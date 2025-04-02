using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Extensions;
using Domain;
using Domain.Flights;
using Domain.Reservations;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using SharedKernel;

namespace Application.Reservations.Create;

public sealed class CreateReservationCommandHandler(
    IApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor) : ICommandHandler<CreateReservationCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateReservationCommand command, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.GetUserId();

        if (userId is null)
            return Result.Failure<Guid>(UserErrors.Unauthenticated);

        var user = await context.Users.FindAsync(userId, cancellationToken);

        if (user is null)
            return Result.Failure<Guid>(UserErrors.NotFound(userId.Value));

        var flight = await context.Flights.FindAsync(command.FlightId, cancellationToken);

        if(flight is null)
            return Result.Failure<Guid>(FlightErrors.NotFound(command.FlightId));

        if(flight.Status is not FlightStatus.Active)
            return Result.Failure<Guid>(FlightErrors.NotActive(flight.Id));

        if(flight.AvailableSeats < command.PassengerCount)
            return Result.Failure<Guid>(FlightErrors.NotEnoughSeats);

        var totalPrice = flight.Price * command.PassengerCount;      
        var reservation = command.ToReservation(userId.Value, totalPrice);

        flight.BookedSeats += command.PassengerCount;
        flight.AvailableSeats -= command.PassengerCount;

        reservation.Raise(new ReservationCreatedDomainEvent(reservation.Id));

        await context.Reservations.AddAsync(reservation, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return reservation.Id;
    }
}