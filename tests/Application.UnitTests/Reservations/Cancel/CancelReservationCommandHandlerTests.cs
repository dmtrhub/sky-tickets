using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Application.Reservations.Cancel;
using Application.UnitTests.Builders;
using Application.UnitTests.Extensions;
using Domain;
using Domain.Reservations;
using Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Reservations.Cancel;

public class CancelReservationCommandHandlerTests
{
    private readonly Mock<IRepository<Reservation>> _reservationRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IRepository<User>> _userRepositoryMock = new();
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();

    private readonly CancelReservationCommandHandler _handler;

    public CancelReservationCommandHandlerTests()
    {
        _handler = new CancelReservationCommandHandler(
            _reservationRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _httpContextAccessorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _httpContextAccessorMock.SetUserId(null);

        var command = new CancelReservationCommand(Guid.NewGuid());

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId, CancellationToken.None))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(UserErrors.Unauthenticated);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenReservationNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _httpContextAccessorMock.SetUserId(userId);

        var mockDbSet = new List<Reservation>().AsQueryable().BuildMockDbSet();

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId, CancellationToken.None))
            .ReturnsAsync(new UserBuilder().WithId(userId).Build());

        _reservationRepositoryMock
            .Setup(x => x.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        var command = new CancelReservationCommand(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ReservationErrors.NotFound(command.Id));
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenDepartureIsLessThan24HoursAway()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _httpContextAccessorMock.SetUserId(userId);

        var reservation = new ReservationBuilder()
           .WithUserId(userId)
           .Build();

        var flight = new FlightBuilder()
            .WithDepartureTime(DateTime.UtcNow.AddHours(5))
            .WithReservation(reservation)
            .Build();
        
        var mockDbSet = new List<Reservation> { reservation }.AsQueryable().BuildMockDbSet();

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId, CancellationToken.None))
            .ReturnsAsync(new UserBuilder().WithId(userId).Build());

        _reservationRepositoryMock
            .Setup(x => x.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        var command = new CancelReservationCommand(reservation.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ReservationErrors.CannotCancel);
    }

    [Fact]
    public async Task Handle_ShouldCancelReservation_WhenAllConditionsAreMet()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _httpContextAccessorMock.SetUserId(userId);

        var reservation = new ReservationBuilder()
           .WithUserId(userId)
           .Build();

        var flight = new FlightBuilder()
            .WithDepartureTime(DateTime.UtcNow.AddDays(2))
            .WithReservation(reservation)
            .Build();

        var mockDbSet = new List<Reservation> { reservation }.AsQueryable().BuildMockDbSet();

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId, CancellationToken.None))
            .ReturnsAsync(new UserBuilder().WithId(userId).Build());

        _reservationRepositoryMock
            .Setup(x => x.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        var command = new CancelReservationCommand(reservation.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(reservation.Id);
        reservation.Status.Should().Be(ReservationStatus.Canceled);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}
