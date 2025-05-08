using Application.Abstractions.Repositories;
using Application.Flights.GetUserFlights;
using Domain.Flights;
using Domain.Reservations;
using Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MockQueryable.Moq;
using Moq;
using System.Security.Claims;

namespace Application.UnitTests.Flights.GetUserFlights;

public class GetUserFlightsQueryHandlerTests
{
    private readonly Mock<IRepository<Flight>> _flightRepositoryMock = new();
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();

    private readonly GetUserFlightsQueryHandler _handler;

    public GetUserFlightsQueryHandlerTests()
    {
        _handler = new GetUserFlightsQueryHandler(
            _flightRepositoryMock.Object,
            _httpContextAccessorMock.Object
        );
    }

    [Fact]
    public async Task Should_ReturnFlights_When_UserHasReservations()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUserIdInContext(userId);

        var flightId = Guid.NewGuid();
        var reservation = Reservation.Create(userId, flightId, 5);

        var flights = new List<Flight>
        {
            new FlightBuilder()
                .WithId(flightId)
                .WithReservation(reservation)
                .Build()
        };

        var mockDbSet = flights.AsQueryable().BuildMockDbSet();

        _flightRepositoryMock
            .Setup(r => r.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        var query = new GetUserFlightsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
    }

    [Fact]
    public async Task Should_Fail_When_UserIsNotAuthenticated()
    {
        // Arrange
        _httpContextAccessorMock.Setup(x => x.HttpContext!.User).Returns(new ClaimsPrincipal());

        var query = new GetUserFlightsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(UserErrors.Unauthenticated.Code);
    }

    [Fact]
    public async Task Should_Fail_When_UserHasNoFlights()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUserIdInContext(userId);

        var flights = new List<Flight>();

        var mockDbSet = flights.AsQueryable().BuildMockDbSet();

        _flightRepositoryMock
            .Setup(r => r.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        var query = new GetUserFlightsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(FlightErrors.NoFlightsFound.Code);
    }

    private void SetUserIdInContext(Guid userId)
    {
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, userId.ToString()) };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        _httpContextAccessorMock.Setup(x => x.HttpContext!.User).Returns(principal);
    }
}
