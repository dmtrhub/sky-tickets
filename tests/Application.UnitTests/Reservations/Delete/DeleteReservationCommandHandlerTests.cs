using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Application.Reservations.Delete;
using Application.UnitTests.Builders;
using Domain.Reservations;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Reservations.Delete;

public class DeleteReservationCommandHandlerTests
{
    private readonly Mock<IRepository<Reservation>> _reservationRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly DeleteReservationCommandHandler _handler;

    public DeleteReservationCommandHandlerTests()
    {
        _handler = new DeleteReservationCommandHandler(
            _reservationRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ReservationExists_DeletesReservation()
    {
        // Arrange
        var reservation = new ReservationBuilder().Build();

        var command = new DeleteReservationCommand(reservation.Id);

        _reservationRepositoryMock
            .Setup(x => x.GetByIdAsync(reservation.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(reservation);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(reservation.Id);

        _reservationRepositoryMock.Verify(x => x.Remove(reservation), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ReservationDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new DeleteReservationCommand(Guid.NewGuid());

        _reservationRepositoryMock
            .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Reservation?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(ReservationErrors.NotFound(command.Id).Code);
    }
}
