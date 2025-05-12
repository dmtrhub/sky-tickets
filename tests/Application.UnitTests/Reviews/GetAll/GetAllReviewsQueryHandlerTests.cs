using Application.Abstractions.Repositories;
using Application.Reviews.GetAll;
using Application.UnitTests.Builders;
using Domain.Reviews;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Reviews.GetAll;

public class GetAllReviewsQueryHandlerTests
{
    private readonly Mock<IRepository<Review>> _reviewRepositoryMock = new();

    private readonly GetAllReviewsQueryHandler _handler;

    public GetAllReviewsQueryHandlerTests()
    {
        _handler = new GetAllReviewsQueryHandler(
            _reviewRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnReviews_WhenReviewsExist()
    {
        // Arrange
        var mockDbSet = new List<Review>
        {
            new ReviewBuilder().Build(),
            new ReviewBuilder().Build()
        }
        .AsQueryable().BuildMockDbSet();

        _reviewRepositoryMock.Setup(repo => repo.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        var query = new GetAllReviewsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_ShouldReturnNoReviewsFound_WhenNoReviewsExist()
    {
        // Arrange
        var mockDbSet = new List<Review>().AsQueryable().BuildMockDbSet();

        _reviewRepositoryMock.Setup(repo => repo.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        var query = new GetAllReviewsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.NoReviewsFound);
    }
}
