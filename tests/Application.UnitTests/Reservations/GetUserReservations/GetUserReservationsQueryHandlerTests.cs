using Application.Abstractions.Repositories;
using Application.Reservations.GetUserReservations;
using Application.UnitTests.Builders;
using Application.UnitTests.Extensions;
using Domain.Reservations;
using Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MockQueryable.Moq;
using Moq;
using System.Security.Claims;

namespace Application.UnitTests.Reservations.GetUserReservations;

public class GetUserReservationsQueryHandlerTests
{
    private readonly Mock<IRepository<Reservation>> _reservationRepositoryMock = new();
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();

    private readonly GetUserReservationsQueryHandler _handler;

    public GetUserReservationsQueryHandlerTests()
    {
        _handler = new GetUserReservationsQueryHandler(
            _reservationRepositoryMock.Object, 
            _httpContextAccessorMock.Object);
    }

    [Fact]
    public async Task Handle_UserIsAuthenticated_ReturnsReservations()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _httpContextAccessorMock.SetUserId(userId);

        var reservation = new ReservationBuilder()
            .WithUserId(userId)
            .Build();

        var mockDbSet = new List<Reservation> { reservation }.AsQueryable().BuildMockDbSet();

        _reservationRepositoryMock
            .Setup(x => x.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        var query = new GetUserReservationsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_UserIsNotAuthenticated_ReturnsError()
    {
        // Arrange
        _httpContextAccessorMock
            .Setup(x => x.HttpContext!.User)
            .Returns(new ClaimsPrincipal());

        var query = new GetUserReservationsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.Unauthenticated);
    }

    [Fact]
    public async Task Handle_NoReservationsFound_ReturnsError()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _httpContextAccessorMock.SetUserId(userId);

        var mockDbSet = new List<Reservation>().AsQueryable().BuildMockDbSet();

        _reservationRepositoryMock
            .Setup(x => x.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        var query = new GetUserReservationsQuery();
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(ReservationErrors.NoReservationsFound.Code);
    }
}
