using Application.Abstractions.Repositories;
using Application.Flights.GetById;
using Application.UnitTests.Builders;
using Domain;
using Domain.Flights;
using Domain.Reservations;
using Domain.Users;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Flights.GetById;

public class GetFlightByIdQueryHandlerTests
{
    private readonly Mock<IRepository<Flight>> _flightRepositoryMock = new();

    private readonly GetFlightByIdQueryHandler _handler;

    public GetFlightByIdQueryHandlerTests()
    {
        _handler = new GetFlightByIdQueryHandler(
            _flightRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenFlightNotFound()
    {
        // Arrange
        var query = new GetFlightByIdQuery(Guid.NewGuid());

        var mockDbSet = new List<Flight>().AsQueryable().BuildMockDbSet();

        _flightRepositoryMock
            .Setup(r => r.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(FlightErrors.NotFound(query.Id).Code);
    }

    [Fact]
    public async Task Handle_ShouldReturnFlight_WhenFlightExists()
    {
        // Arrange
        var flight = new FlightBuilder()
                .WithReservation(new ReservationBuilder().Build())
                .Build();

        var mockDbSet = new List<Flight> { flight }.AsQueryable().BuildMockDbSet();

        _flightRepositoryMock
            .Setup(r => r.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        var query = new GetFlightByIdQuery(flight.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(flight.Id);
    }
}
