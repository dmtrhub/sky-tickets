using Application.Abstractions.Repositories;
using Application.Flights.GetUserFlights;
using Application.UnitTests.Builders;
using Application.UnitTests.Extensions;
using Domain.Flights;
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
        _httpContextAccessorMock.SetUserId(userId);

        var reservation = new ReservationBuilder()
            .WithPassengerCount(5)
            .WithUserId(userId)
            .Build();

        var flight = new FlightBuilder()
                .WithReservation(reservation)
                .Build();

        var mockDbSet = new List<Flight> { flight }.AsQueryable().BuildMockDbSet();

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
        _httpContextAccessorMock
            .Setup(x => x.HttpContext!.User)
            .Returns(new ClaimsPrincipal());

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
        _httpContextAccessorMock.SetUserId(userId);

        var mockDbSet = new List<Flight>().AsQueryable().BuildMockDbSet();

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
}
