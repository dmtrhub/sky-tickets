using SharedKernel;

namespace Domain;

public static class AirlineErrors
{
    public static Error NotFound(Guid airlineId) =>
        Error.NotFound("Airline.NotFound", $"Airline with id {airlineId} was not found");  
}
