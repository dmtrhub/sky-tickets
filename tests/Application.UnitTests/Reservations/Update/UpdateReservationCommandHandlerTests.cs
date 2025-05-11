using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Application.Reservations.Update;
using Application.UnitTests.Builders;
using Domain.Reservations;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Reservations.Update;

public class UpdateReservationCommandHandlerTests
{
    private readonly Mock<IRepository<Reservation>> _reservationRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly UpdateReservationCommandHandler _handler;

    public UpdateReservationCommandHandlerTests()
    {
        _reservationRepositoryMock = new Mock<IRepository<Reservation>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new UpdateReservationCommandHandler(
            _reservationRepositoryMock.Object, 
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenReservationDoesNotExist()
    {
        // Arrange
        var command = new UpdateReservationCommand(Guid.NewGuid(), 3, "Active");

        _reservationRepositoryMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Reservation?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ReservationErrors.NotFound(command.Id));
    }

    [Fact]
    public async Task Handle_ShouldUpdateReservation_WhenReservationExists()
    {
        // Arrange
        var reservation = new ReservationBuilder().Build();

        var command = new UpdateReservationCommand(reservation.Id, 2, "Canceled");

        _reservationRepositoryMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(reservation);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(reservation.Id);
    }
}
