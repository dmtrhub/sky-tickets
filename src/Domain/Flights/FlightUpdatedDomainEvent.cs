using SharedKernel;

namespace Domain.Flights;

public sealed record FlightUpdatedDomainEvent(Guid Id, string Departure, string Destination) : IDomainEvent;
