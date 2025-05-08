using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Application.Flights.Create;
using Domain.Flights;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Flights.Create;

public class CreateFlightCommandHandlerTests
{
    private readonly Mock<IRepository<Flight>> _flightRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly CreateFlightCommandHandler _handler;

    public CreateFlightCommandHandlerTests()
    {
        _handler = new CreateFlightCommandHandler(
            _flightRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateFlight_AndReturnId()
    {
        // Arrange
        var command = new CreateFlightCommand(
            Guid.NewGuid(),
            "New York",
            "Los Angeles",
            DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm"),
            DateTime.UtcNow.AddHours(2).ToString("yyyy-MM-dd HH:mm"),
            200,
            148.0m);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        _flightRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Flight>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
