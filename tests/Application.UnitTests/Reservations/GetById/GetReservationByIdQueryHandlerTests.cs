using Application.Abstractions.Repositories;
using Application.Reservations.GetById;
using Application.UnitTests.Builders;
using Domain.Reservations;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Reservations.GetById;

public class GetReservationByIdQueryHandlerTests
{
    private readonly Mock<IRepository<Reservation>> _reservationRepositoryMock = new();

    private readonly GetReservationByIdQueryHandler _handler;

    public GetReservationByIdQueryHandlerTests()
    {
        _handler = new GetReservationByIdQueryHandler(
            _reservationRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnReservation_WhenReservationWithIdExist()
    {
        // Arrange
        var reservation = new ReservationBuilder().Build();

        var mockDbSet = new List<Reservation> { reservation }.AsQueryable()
            .BuildMockDbSet();

        _reservationRepositoryMock
            .Setup(r => r.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        var query = new GetReservationByIdQuery(reservation.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(reservation.Id);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenReservationWithIdDoesNotExist()
    {
        // Arrange
        var query = new GetReservationByIdQuery(Guid.NewGuid());

        var mockDbSet = new List<Reservation>().AsQueryable()
            .BuildMockDbSet();

        _reservationRepositoryMock
            .Setup(r => r.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(ReservationErrors.NotFound(query.Id).Code);
    }
}
