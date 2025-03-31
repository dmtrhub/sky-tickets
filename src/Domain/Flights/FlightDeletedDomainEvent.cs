using SharedKernel;

namespace Domain.Flights;

public sealed record FlightDeletedDomainEvent(Guid Id, string Departure, string Destination) : IDomainEvent;
