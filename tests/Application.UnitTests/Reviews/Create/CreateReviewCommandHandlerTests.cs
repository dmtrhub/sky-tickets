using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Application.Reviews.Create;
using Application.UnitTests.Builders;
using Application.UnitTests.Extensions;
using Domain;
using Domain.Reservations;
using Domain.Reviews;
using Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MockQueryable.Moq;
using Moq;
using System.Linq.Expressions;

namespace Application.UnitTests.Reviews.Create;

public class CreateReviewCommandHandlerTests
{
    private readonly Mock<IRepository<Review>> _reviewRepositoryMock = new();
    private readonly Mock<IRepository<User>> _userRepositoryMock = new();
    private readonly Mock<IRepository<Reservation>> _reservationRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();

    private readonly CreateReviewCommandHandler _handler;

    public CreateReviewCommandHandlerTests()
    {
        _handler = new CreateReviewCommandHandler(
            _reviewRepositoryMock.Object,
            _userRepositoryMock.Object,
            _reservationRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _httpContextAccessorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthenticated_WhenUserIdIsNull()
    {
        // Arrange
        _httpContextAccessorMock.SetUserId(null);

        var command = new CreateReviewCommand(Guid.NewGuid(), "Test Review", "Test Content", null);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.Unauthenticated);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserNotExists()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _httpContextAccessorMock.SetUserId(userId);

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var command = new CreateReviewCommand(Guid.NewGuid(), "Test Review", "Test Content", null);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.NotFound(userId));
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNoCompletedFlights_ForAirline()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _httpContextAccessorMock.SetUserId(userId);

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserBuilder().WithId(userId).Build());

        var reservationMockDbSet = new List<Reservation>().AsQueryable().BuildMockDbSet();

        _reservationRepositoryMock
            .Setup(x => x.AsQueryable())
            .ReturnsAsync(reservationMockDbSet.Object);

        var command = new CreateReviewCommand(new AirlineBuilder().Build().Id, "Test Review", "Test Content", null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.NoCompletedFlightForAirline(command.AirlineId));
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenReviewAlreadyExists()
    {
        // Arrange
        var user = new UserBuilder().Build();

        _httpContextAccessorMock.SetUserId(user.Id);

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var flight = new FlightBuilder()
            .WithStatus(FlightStatus.Completed)
            .Build();

        var reservation = new ReservationBuilder()
            .WithUserId(user.Id)
            .WithFlight(flight)
            .Build();

        var reservationMockDbSet = new List<Reservation> { reservation }.AsQueryable().BuildMockDbSet();

        _reservationRepositoryMock
            .Setup(x => x.AsQueryable())
            .ReturnsAsync(reservationMockDbSet.Object);

        _reviewRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Review, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new CreateReviewCommand(flight.AirlineId, "Test Review", "Test Content", null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.AlreadyReviewed(flight.AirlineId));
    }

    [Fact]
    public async Task Handle_ShouldCreateReviewSuccessfully()
    {
        // Arrange
        var user = new UserBuilder().Build();

        _httpContextAccessorMock.SetUserId(user.Id);

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var flight = new FlightBuilder()
            .WithStatus(FlightStatus.Completed)
            .Build();

        var reservation = new ReservationBuilder()
            .WithUserId(user.Id)
            .WithFlight(flight)
            .Build();

        var reservationMockDbSet = new List<Reservation> { reservation }.AsQueryable().BuildMockDbSet();

        _reservationRepositoryMock
            .Setup(x => x.AsQueryable())
            .ReturnsAsync(reservationMockDbSet.Object);

        _reviewRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Review, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = new CreateReviewCommand(flight.AirlineId, "Test Review", "Test Content", null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
