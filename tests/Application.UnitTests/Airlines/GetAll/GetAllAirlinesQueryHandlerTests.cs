using Application.Abstractions.Repositories;
using Application.Airlines.GetAll;
using Domain.Airlines;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Airlines.GetAll;

public class GetAllAirlinesQueryHandlerTests
{
    private readonly Mock<IRepository<Airline>> _airlineRepositoryMock = new();

    private readonly GetAllAirlinesQueryHandler _handler;

    public GetAllAirlinesQueryHandlerTests()
    {
        _handler = new GetAllAirlinesQueryHandler(_airlineRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNoAirlinesFound()
    {
        // Arrange
        var mockDbSet = new List<Airline>().AsQueryable().BuildMockDbSet();

        _airlineRepositoryMock.Setup(r => r.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        var query = new GetAllAirlinesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(AirlineErrors.NoAirlinesFound.Code);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfAirlineResponses_WhenAirlinesExist()
    {
        // Arrange
        var airline = Airline.Create("TestAirline", "TestAddress", "TestInfo");
        var airlines = new List<Airline> { airline };

        var mockDbSet = airlines.AsQueryable().BuildMockDbSet();

        _airlineRepositoryMock.Setup(r => r.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        var query = new GetAllAirlinesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().ContainSingle();
        result.Value[0].Name.Should().Be("TestAirline");
    }
}
