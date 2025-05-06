using Application.Abstractions.Repositories;
using Application.Airlines.GetById;
using Domain.Airlines;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Airlines.GetById;

public class GetAirlineByIdQueryHandlerTests
{
    private readonly Mock<IRepository<Airline>> _airlineRepositoryMock = new();

    private readonly GetAirlineByIdQueryHandler _handler;

    public GetAirlineByIdQueryHandlerTests()
    {
        _handler = new GetAirlineByIdQueryHandler(_airlineRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAirlineNotFound()
    {
        // Arrange
        var query = new GetAirlineByIdQuery(Guid.NewGuid());

        var mockDbSet = new List<Airline>().AsQueryable().BuildMockDbSet();

        _airlineRepositoryMock
            .Setup(r => r.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(AirlineErrors.NotFound(query.Id).Code);
    }

    [Fact]
    public async Task Handle_ShouldReturnAirlineResponse_WhenAirlineExists()
    {
        // Arrange
        var airline = Airline.Create("TestAirline", "TestAddress", "TestInfo");
        var airlines = new List<Airline> { airline };

        var mockDbSet = airlines.AsQueryable().BuildMockDbSet();

        _airlineRepositoryMock
            .Setup(r => r.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        var query = new GetAirlineByIdQuery(airline.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("TestAirline");
    }
}

