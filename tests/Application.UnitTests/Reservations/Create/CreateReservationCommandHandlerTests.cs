using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Application.Reservations.Create;
using Application.UnitTests.Builders;
using Application.UnitTests.Extensions;
using Domain;
using Domain.Flights;
using Domain.Reservations;
using Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Application.UnitTests.Reservations.Create;

public class CreateReservationCommandHandlerTests
{
    private readonly Mock<IRepository<Reservation>> _reservationRepositoryMock = new();
    private readonly Mock<IRepository<User>> _userRepositoryMock = new();
    private readonly Mock<IRepository<Flight>> _flightRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();

    private readonly CreateReservationCommandHandler _handler;

    public CreateReservationCommandHandlerTests()
    {
        _handler = new CreateReservationCommandHandler(
            _reservationRepositoryMock.Object,
            _userRepositoryMock.Object,
            _flightRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _httpContextAccessorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthenticated_WhenUserIdIsNull()
    {
        // Arrange
        _httpContextAccessorMock.SetUserId(null);

        var command = new CreateReservationCommand(Guid.NewGuid(), 1);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.Unauthenticated);
    }

    [Fact]
    public async Task Should_Return_NotFound_When_User_Not_Exists()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _httpContextAccessorMock.SetUserId(userId);

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId, default))
            .ReturnsAsync((User?)null);

        var command = new CreateReservationCommand(Guid.NewGuid(), 1);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.NotFound(userId));
    }

    [Fact]
    public async Task Should_Return_FlightNotFound_When_Flight_Not_Exists()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _httpContextAccessorMock.SetUserId(userId);  

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId, default))
            .ReturnsAsync(new UserBuilder().Build());

        var flightId = Guid.NewGuid();

        _flightRepositoryMock
            .Setup(x => x.GetByIdAsync(flightId, default))
            .ReturnsAsync((Flight?)null);

        var command = new CreateReservationCommand(flightId, 1);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FlightErrors.NotFound(flightId));
    }

    [Fact]
    public async Task Should_Return_FlightNotActive_When_Flight_Status_Not_Active()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _httpContextAccessorMock.SetUserId(userId);

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId, default))
            .ReturnsAsync(new UserBuilder().Build());

        var flight = new FlightBuilder()
                        .WithStatus(FlightStatus.Canceled)
                        .Build();

        _flightRepositoryMock
            .Setup(x => x.GetByIdAsync(flight.Id, default))
            .ReturnsAsync(flight);

        var command = new CreateReservationCommand(flight.Id, 1);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FlightErrors.NotActive(flight.Id));
    }

    [Fact]
    public async Task Should_Return_NotEnoughSeats_When_Flight_Has_Less_Seats()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _httpContextAccessorMock.SetUserId(userId);

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId, default))
            .ReturnsAsync(new UserBuilder().Build);

        var flight = new FlightBuilder()
                        .WithAvailableSeats(2)
                        .Build();

        _flightRepositoryMock
            .Setup(x => x.GetByIdAsync(flight.Id, default))
            .ReturnsAsync(flight);

        var command = new CreateReservationCommand(flight.Id, 3);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FlightErrors.NotEnoughSeats);
    }

    [Fact]
    public async Task Should_Create_Reservation_Successfully()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _httpContextAccessorMock.SetUserId(userId);

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId, default))
            .ReturnsAsync(new UserBuilder().Build);

        var flight = new FlightBuilder()
                    .WithAvailableSeats(5)
                    .Build();

        _flightRepositoryMock
            .Setup(x => x.GetByIdAsync(flight.Id, default))
            .ReturnsAsync(flight);

        var command = new CreateReservationCommand(flight.Id, 2);

        _reservationRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Reservation>(), default))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(default))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        flight.AvailableSeats.Should().Be(3);
        flight.BookedSeats.Should().Be(2);
    }
}
