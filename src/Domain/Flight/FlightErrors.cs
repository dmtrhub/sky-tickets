using SharedKernel;

namespace Domain;

public static class FlightErrors
{
    public static Error NotFound(Guid flightId) =>
        Error.NotFound("Flight.NotFound", $"Flight with id {flightId} was not found");

    public static Error NotActive(Guid flightId) =>
        Error.Problem("Flight.NotActive", $"Flight with id {flightId} is not active");

    public static Error NotCanceled(Guid airlineId) =>
        Error.Problem("Flight.NotCanceled", $"Flight with id {airlineId} was not canceled");

    public static Error NotCompleted(Guid airlineId) =>
        Error.Problem("Flight.NotCompleted", $"Flight with id {airlineId} was not completed");

    public static readonly Error NoFlightsFound =
        Error.NotFound("Flight.NoFlightFound", $"No flights found in the system");
}
