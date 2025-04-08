using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Application.Airlines.Create;
using Application.Airlines;
using Domain.Airlines;
using Moq;
using System.Linq.Expressions;

namespace Application.UnitTests.Airlines.Create;

public class CreateAirlineCommandHandlerTests
{
    private readonly Mock<IRepository<Airline>> _airlineRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateAirlineCommandHandler _handler;

    public CreateAirlineCommandHandlerTests()
    {
        _airlineRepositoryMock = new Mock<IRepository<Airline>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateAirlineCommandHandler(_airlineRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAirlineNameIsAlreadyTaken()
    {
        // Arrange
        var command = new CreateAirlineCommand("Existing Airline", "Some Address", "123-456");
        _airlineRepositoryMock
            .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Airline, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);  // Simulating that the airline already exists

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AirlineErrors.NameInUse(command.Name), result.Error);
    }

    [Fact]
    public async Task Handle_ShouldCreateAirline_WhenValidCommand()
    {
        // Arrange
        var command = new CreateAirlineCommand("New Airline", "Some Address", "123-456");
        var airline = command.ToAirline();

        _airlineRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Airline>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _airlineRepositoryMock.Verify(r => r.AddAsync(It.Is<Airline>(a => a.Name == command.Name), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
