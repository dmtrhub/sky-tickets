using SharedKernel;

namespace Domain.Flights;

public sealed record FlightCompletedDomainEvent(Guid Id, string Departure, string Destination) : IDomainEvent;
