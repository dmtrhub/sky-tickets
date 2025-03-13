using SharedKernel;

namespace Domain;

public static class ReservationErrors
{
    public static Error NotFound(Guid reservationId) =>
        Error.NotFound("Reservation.NotFound", $"Reservation with id {reservationId} was not found");

    public static Error NotCreated(Guid userId, Guid flightId) =>
        Error.Problem("Reservation.NotCreated", $"Reservation for user {userId} and flight {flightId} was not created");

    public static Error NotApproved(Guid reservationId) =>
        Error.Problem("Reservation.NotApproved", $"Reservation with id {reservationId} was not approved");

    public static Error NotCanceled(Guid reservationId) =>
        Error.Problem("Reservation.NotCanceled", $"Reservation with id {reservationId} was not canceled");

    public static Error NotCompleted(Guid reservationId) =>
        Error.Problem("Reservation.NotCompleted", $"Reservation with id {reservationId} was not completed");
}
