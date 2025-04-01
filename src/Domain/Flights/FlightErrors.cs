using SharedKernel;

namespace Domain.Flights;

public static class FlightErrors
{
    public static Error NotFound(Guid flightId) =>
        Error.NotFound("Flight.NotFound", $"Flight with id {flightId} was not found");

    public static Error NotActive(Guid flightId) =>
        Error.Problem("Flight.NotActive", $"Flight with id {flightId} is not active");

    public static Error NotCanceled(Guid flightId) =>
        Error.Problem("Flight.NotCanceled", $"Flight with id {flightId} was not canceled");

    public static Error NotCompleted(Guid flightId) =>
        Error.Problem("Flight.NotCompleted", $"Flight with id {flightId} was not completed");

    public static readonly Error NotEnoughSeats =
        Error.Problem("Flight.NotEnoughSeats", "Not enough available seats on this flight.");

    public static readonly Error NoFlightsFound =
        Error.NotFound("Flight.NoFlightFound", $"No flights found in the system");

    public static readonly Error HasReservations =
        Error.Problem("Flight.HasReservations", "Cannot delete flight with reservations");
}
