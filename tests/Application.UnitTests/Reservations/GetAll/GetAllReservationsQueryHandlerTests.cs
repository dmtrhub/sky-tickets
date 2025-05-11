using Application.Abstractions.Repositories;
using Application.Reservations.GetAll;
using Application.UnitTests.Builders;
using Domain.Reservations;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Reservations.GetAll;

public class GetAllReservationsQueryHandlerTests
{
    private readonly Mock<IRepository<Reservation>> _reservationRepositoryMock = new();

    private readonly GetAllReservationsQueryHandler _handler;
    public GetAllReservationsQueryHandlerTests()
    {
        _handler = new GetAllReservationsQueryHandler(
            _reservationRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNoReservationsFound()
    {
        // Arrange
        var query = new GetAllReservationsQuery();

        var mockDbSet = new List<Reservation>().AsQueryable().BuildMockDbSet();

        _reservationRepositoryMock
            .Setup(x => x.AsQueryable())
            .ReturnsAsync(mockDbSet.Object
            );

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReservationErrors.NoReservationsFound);
    }

    [Fact]
    public async Task Handle_ShouldReturnReservations_WhenReservationsExist()
    {
        // Arrange
        var query = new GetAllReservationsQuery();

        var userId = new UserBuilder().Build().Id;
        var flightId = new FlightBuilder().Build().Id;

        var reservation = new ReservationBuilder().Build();

        var mockDbSet = new List<Reservation> { reservation }.AsQueryable().BuildMockDbSet();

        _reservationRepositoryMock
            .Setup(x => x.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(1);
    }
}
