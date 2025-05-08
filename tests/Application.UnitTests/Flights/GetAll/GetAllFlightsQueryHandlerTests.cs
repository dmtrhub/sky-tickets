using Application.Abstractions.Repositories;
using Application.Flights.GetAll;
using Domain;
using Domain.Flights;
using Domain.Reservations;
using Domain.Users;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Flights.GetAll;

public class GetAllFlightsQueryHandlerTests
{
    private readonly Mock<IRepository<Flight>> _flightRepositoryMock = new();

    private readonly GetAllFlightsQueryHandler _handler;

    public GetAllFlightsQueryHandlerTests()
    {
        _handler = new GetAllFlightsQueryHandler(
            _flightRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_ReturnFailure_When_NoFlightsFound()
    {
        // Arrange
        var query = new GetAllFlightsQuery();

        var mockDbSet = new List<Flight>().AsQueryable().BuildMockDbSet();

        _flightRepositoryMock
            .Setup(r => r.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(FlightErrors.NoFlightsFound.Code);
    }

    [Fact]
    public async Task Should_ReturnFlights_When_FlightsExist()
    {
        // Arrange
        var query = new GetAllFlightsQuery();

        var user = User.Create("Test", "User", "test@test.com", "hashed", DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-20)), Gender.Male);

        var flightId1 = Guid.NewGuid();
        var flightId2 = Guid.NewGuid();

        var flights = new List<Flight>()
        {
            new FlightBuilder()
                .WithId(flightId1)
                .WithReservation(Reservation.Create(user.Id, flightId1, 25))
                .Build(),
            new FlightBuilder()
                .WithId(flightId2)
                .WithReservation(Reservation.Create(user.Id, flightId2, 30))
                .Build(),
        };

        var mockDbSet = flights.AsQueryable().BuildMockDbSet();

        _flightRepositoryMock
            .Setup(r => r.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(flights.Count);
        result.Value[0].AirlineName.Should().Be("Test Airline");
    }
}
