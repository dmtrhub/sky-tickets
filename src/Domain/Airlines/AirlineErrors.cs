using SharedKernel;

namespace Domain.Airlines;

public static class AirlineErrors
{
    public static Error NotFound(Guid airlineId) =>
        Error.NotFound("Airlines.NotFound", $"Airline with id {airlineId} was not found");

    public static readonly Error NoAirlinesFound =
        Error.NotFound("Airlines.NoAirlineFound", "No airlines found in the system");

    public static readonly Error CannotDeleteWithActiveFlights =
        Error.Conflict("Airlines.CannotDeleteWithActiveFlights", "Cannot delete airline with active flights");
    public static Error NameInUse(string name) =>
        Error.Conflict("Airlines.NameInUse", $"The name '{name}' is already in use");
}
