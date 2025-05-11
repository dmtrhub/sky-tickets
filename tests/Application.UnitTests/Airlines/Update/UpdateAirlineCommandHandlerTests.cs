using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Application.Airlines.Update;
using Application.UnitTests.Builders;
using Domain.Airlines;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Airlines.Update;

public class UpdateAirlineCommandHandlerTests
{
    private readonly Mock<IRepository<Airline>> _airlineRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly UpdateAirlineCommandHandler _handler;

    public UpdateAirlineCommandHandlerTests()
    {
        _handler = new UpdateAirlineCommandHandler(
            _airlineRepositoryMock.Object, 
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAirlineNotFound()
    {
        // Arrange
        var command = new UpdateAirlineCommand(Guid.NewGuid(), "NewAirline", "NewAddress", "NewInfo");

        _airlineRepositoryMock
            .Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Airline?)null); 

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(AirlineErrors.NotFound(command.Id).Code);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAirlineNameIsInUse()
    {
        // Arrange
        var existingAirline = new AirlineBuilder()
            .WithName("ExistingAirline")
            .Build();
        var command = new UpdateAirlineCommand(existingAirline.Id, "ExistingAirline", "NewAddress", "NewInfo");

        _airlineRepositoryMock
            .Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingAirline);

        _airlineRepositoryMock
            .Setup(r => r.AnyAsync(a => a.Name == command.Name && a.Id != command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(AirlineErrors.NameInUse(command.Name!).Code);
    }

    [Fact]
    public async Task Handle_ShouldReturnAirlineId_WhenAirlineUpdatedSuccessfully()
    {
        // Arrange
        var existingAirline = new AirlineBuilder().Build();
        var command = new UpdateAirlineCommand(existingAirline.Id, "UpdatedAirline", "UpdatedAddress", "UpdatedInfo");

        _airlineRepositoryMock
            .Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingAirline);

        _airlineRepositoryMock
            .Setup(r => r.AnyAsync(a => a.Name == command.Name && a.Id != command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(existingAirline.Id);

        _unitOfWorkMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
