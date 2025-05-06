using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Application.Airlines.Delete;
using Domain.Airlines;
using Domain.Flights;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Airlines.Delete;

public class DeleteAirlineCommandHandlerTests
{
    private readonly Mock<IRepository<Airline>> _airlineRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly DeleteAirlineCommandHandler _handler;

    public DeleteAirlineCommandHandlerTests()
    {
        _handler = new DeleteAirlineCommandHandler(_airlineRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAirlineNotFound()
    {
        // Arrange
        var command = new DeleteAirlineCommand(Guid.NewGuid());

        _airlineRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Airline?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(AirlineErrors.NotFound(command.Id).Code);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAirlineHasActiveFlights()
    {
        // Arrange
        var command = new DeleteAirlineCommand(Guid.NewGuid());
        var airline = Airline.Create("TestAirline", "Address", "Contact");

        var departureTime = DateTime.Now.AddHours(1);
        var arrivalTime = DateTime.Now.AddHours(3);

        airline.Flights.Add(Flight.Create(
            airline.Id,
            "FROM", "TO",
            departureTime,
            arrivalTime,
            200,
            150));

        _airlineRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(airline);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(AirlineErrors.CannotDeleteWithActiveFlights.Code);
    }

    [Fact]
    public async Task Handle_ShouldDeleteAirline_WhenValid()
    {
        // Arrange
        var command = new DeleteAirlineCommand(Guid.NewGuid());
        var airline = Airline.Create("TestAirline", "Address", "Contact");
        airline.SetId(command.Id); // private set Id

        _airlineRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(airline);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(airline.Id);

        _airlineRepositoryMock.Verify(r => r.Remove(airline), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}