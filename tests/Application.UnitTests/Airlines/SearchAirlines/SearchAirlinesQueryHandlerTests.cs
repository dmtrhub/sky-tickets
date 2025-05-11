using Application.Abstractions.Repositories;
using Application.Airlines.SearchAirlines;
using Application.UnitTests.Builders;
using Domain.Airlines;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Airlines.SearchAirlines;

public class SearchAirlinesQueryHandlerTests
{
    private readonly Mock<IRepository<Airline>> _airlineRepositoryMock = new();

    private readonly SearchAirlinesQueryHandler _handler;

    public SearchAirlinesQueryHandlerTests()
    {
        _handler = new SearchAirlinesQueryHandler(
            _airlineRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNoAirlinesFound()
    {
        // Arrange
        var query = new SearchAirlinesQuery("NonExistent", "Wrong Address");

        var mockDbSet = new List<Airline>()
            .AsQueryable()
            .BuildMockDbSet();

        _airlineRepositoryMock
            .Setup(r => r.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(AirlineErrors.NoAirlinesFound.Code);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAirlinesFound()
    {
        // Arrange
        var airline = new AirlineBuilder()
            .WithName("AirSerbia")
            .WithAddress("Belgrade")
            .Build();

        var query = new SearchAirlinesQuery("AirSerbia", "Belgrade");

        var mockDbSet = new List<Airline> { airline }
            .AsQueryable()
            .BuildMockDbSet();

        _airlineRepositoryMock
            .Setup(r => r.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().ContainSingle();
        result.Value[0].Name.Should().Be(airline.Name);
    }
}
