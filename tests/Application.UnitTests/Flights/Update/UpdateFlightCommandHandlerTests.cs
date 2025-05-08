using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Application.Flights;
using Application.Flights.Update;
using Domain.Flights;
using Domain.Reservations;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Flights.Update;

public class UpdateFlightCommandHandlerTests
{
    private readonly Mock<IRepository<Flight>> _flightRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly UpdateFlightCommandHandler _handler;

    public UpdateFlightCommandHandlerTests()
    {
        _handler = new UpdateFlightCommandHandler(
            _flightRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenFlightNotFound()
    {
        // Arrange
        var command = new UpdateFlightCommand(Guid.NewGuid(), null, null, null, null, null, null);

        var mockDbSet = new List<Flight>().AsQueryable().BuildMockDbSet();

        _flightRepositoryMock
            .Setup(r => r.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(FlightErrors.NotFound(command.Id).Code);
    }

    [Fact]
    public void Should_NotUpdatePrice_When_HasReservations()
    {
        // Arrange
        var flightId = Guid.NewGuid();

        var reservation = Reservation.Create(Guid.NewGuid(), flightId, 2);

        var flight = new FlightBuilder()
            .WithId(flightId)
            .WithReservation(reservation)
            .Build();

        var command = new UpdateFlightCommand(Guid.NewGuid(), "2025-06-01 08:00", "2025-06-01 10:00", 300, 200, 90, "Completed");     

        // Act
        flight.UpdateFlight(command, true);

        // Assert
        flight.Price.Should().Be(flight.Price);
        flight.Price.Should().NotBe(command.Price);
    }

    [Fact]
    public void Should_UpdatePrice_When_NoReservationsExist()
    {
        // Arrange
        var flight = new FlightBuilder()
            .WithId(Guid.NewGuid())
            .Build();

        var originalPrice = flight.Price;

        var command = new UpdateFlightCommand(Guid.NewGuid(), "2025-06-01 08:00", "2025-06-01 10:00", 300, 200, 90, "Completed");

        // Act
        flight.UpdateFlight(command, false);

        // Assert
        flight.Price.Should().Be(command.Price);
        flight.Price.Should().NotBe(originalPrice);
    }
}
