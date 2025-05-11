using Application.Abstractions.Repositories;
using Application.Flights.SearchActive;
using Application.UnitTests.Builders;
using Domain.Flights;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Flights.SearchActive;

public class SearchActiveFlightsQueryHandlerTests
{
    private readonly Mock<IRepository<Flight>> _flightRepositoryMock = new();

    private readonly SearchActiveFlightsQueryHandler _handler;

    public SearchActiveFlightsQueryHandlerTests()
    {
        _handler = new SearchActiveFlightsQueryHandler(
            _flightRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_ReturnFilteredFlights_ByDestinationAndDate()
    {
        // Arrange
        var targetDate = DateTime.UtcNow.AddDays(10).Date;

        var matchingFlight = new FlightBuilder()
            .WithDestination("Berlin")
            .WithDepartureTime(targetDate)
            .Build();

        var flights = new List<Flight>
        {
            matchingFlight,
            new FlightBuilder()
                .WithId(Guid.NewGuid())
                .WithDestination("Berlin")
                .Build(),
            new FlightBuilder()
                .WithId(Guid.NewGuid())
                .WithDepartureTime(targetDate)
                .Build()
        };

        var mockDbSet = flights.AsQueryable().BuildMockDbSet();

        _flightRepositoryMock
            .Setup(r => r.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        var query = new SearchActiveFlightsQuery("Berlin", targetDate, null, null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        result.Value[0].Destination.Should().Be("Berlin");
        result.Value[0].DepartureTime.Should().Be(targetDate);
    }

    [Fact]
    public async Task Should_ReturnFilteredFlights_BySeatsAndPrice()
    {
        // Arrange
        var matchingFlight = new FlightBuilder()
            .WithAvailableSeats(121)
            .WithPrice(195)
            .Build();

        var flights = new List<Flight>
        {
            matchingFlight,
            new FlightBuilder()
                .WithId(Guid.NewGuid())
                .WithPrice(100)
                .Build(),
            new FlightBuilder()
                .WithId(Guid.NewGuid())
                .WithAvailableSeats(150)
                .Build()
        };

        var mockDbSet = flights.AsQueryable().BuildMockDbSet();

        _flightRepositoryMock
            .Setup(r => r.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        var query = new SearchActiveFlightsQuery(null, null, 120, 200);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        result.Value[0].Price.Should().BeLessThanOrEqualTo(200);
        result.Value[0].AvailableSeats.Should().BeGreaterThanOrEqualTo(120);
    }

    [Fact]
    public async Task Should_ReturnFailure_When_NoFlightsMatchCriteria()
    {
        // Arrange
        var flight = new FlightBuilder().Build();

        var mockDbSet = new List<Flight> { flight }.AsQueryable().BuildMockDbSet();

        _flightRepositoryMock
            .Setup(r => r.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        var query = new SearchActiveFlightsQuery("Moscow", DateTime.UtcNow.AddDays(5), 120, 200);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(FlightErrors.NoFlightsFound.Code);
    }
}
