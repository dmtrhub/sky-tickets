using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Application.Flights.Delete;
using Domain.Flights;
using Domain.Reservations;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Flights.Delete;

public class DeleteFlightCommandHandlerTests
{
    private readonly Mock<IRepository<Flight>> _flightRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly DeleteFlightCommandHandler _handler;

    public DeleteFlightCommandHandlerTests()
    {
        _handler = new DeleteFlightCommandHandler(
            _flightRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenFlightNotFound()
    {
        // Arrange
        var command = new DeleteFlightCommand(Guid.NewGuid());

        _flightRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Flight?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(FlightErrors.NotFound(command.Id).Code);
    }

    [Fact]
    public async Task Should_ReturnFailure_When_Flight_Has_Active_Reservations()
    {
        var command = new DeleteFlightCommand(Guid.NewGuid());

        var flight = new FlightBuilder()
            .WithId(command.Id)
            .WithReservation(Reservation.Create(
                Guid.NewGuid(),
                command.Id,
                20))
            .Build();

        _flightRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, default)).ReturnsAsync(flight);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(FlightErrors.HasReservations.Code);
    }

    [Fact]
    public async Task Should_Delete_Flight_When_Valid()
    {
        var command = new DeleteFlightCommand(Guid.NewGuid());

        var flight = new FlightBuilder()
            .WithId(command.Id)
            .WithNoReservations()
            .Build();

        _flightRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, default)).ReturnsAsync(flight);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(command.Id);

        _flightRepositoryMock.Verify(r => r.Remove(flight), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

}
