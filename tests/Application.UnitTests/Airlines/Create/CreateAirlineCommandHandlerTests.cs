using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Application.Airlines.Create;
using Domain.Airlines;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace Application.UnitTests.Airlines.Create;

public class CreateAirlineCommandHandlerTests
{
    private readonly Mock<IRepository<Airline>> _airlineRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly CreateAirlineCommandHandler _handler;

    public CreateAirlineCommandHandlerTests()
    {
        _handler = new CreateAirlineCommandHandler(_airlineRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAirlineNameIsAlreadyTaken()
    {
        // Arrange
        var command = new CreateAirlineCommand("Existing Airline", "Some Address", "123-456");

        _airlineRepositoryMock
            .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Airline, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(AirlineErrors.NameInUse(command.Name).Code);
    }

    [Fact]
    public async Task Handle_ShouldAddAirline_WhenNameIsAvailable()
    {
        // Arrange
        var command = new CreateAirlineCommand("New Airline", "Some Address", "123-456");

        _airlineRepositoryMock
            .Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Airline, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _airlineRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Airline>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);


        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _airlineRepositoryMock
            .Verify(r => r.AddAsync(It.IsAny<Airline>(), It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock
            .Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
