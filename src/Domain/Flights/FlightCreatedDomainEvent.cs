using SharedKernel;

namespace Domain.Flights;

public sealed record FlightCreatedDomainEvent(Guid Id, string Departure, string Destination) : IDomainEvent;
