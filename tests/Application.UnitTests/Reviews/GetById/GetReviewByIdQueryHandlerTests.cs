using Application.Abstractions.Repositories;
using Application.Reviews.GetById;
using Application.UnitTests.Builders;
using Domain.Reviews;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Reviews.GetById;

public class GetReviewByIdQueryHandlerTests
{
    private readonly Mock<IRepository<Review>> _reviewRepositoryMock = new();
    private readonly GetReviewByIdQueryHandler _handler;

    public GetReviewByIdQueryHandlerTests()
    {
        _reviewRepositoryMock = new Mock<IRepository<Review>>();
        _handler = new GetReviewByIdQueryHandler(
            _reviewRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnReview_WhenReviewWithIdExist()
    {
        // Arrange
        var review = new ReviewBuilder().Build();

        var mockDbSet = new List<Review> { review }.AsQueryable()
            .BuildMockDbSet();

        _reviewRepositoryMock
            .Setup(r => r.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        var query = new GetReviewByIdQuery(review.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(review.Id);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenReviewWithIdDoesNotExist()
    {
        // Arrange
        var query = new GetReviewByIdQuery(Guid.NewGuid());

        var mockDbSet = new List<Review>().AsQueryable()
            .BuildMockDbSet();

        _reviewRepositoryMock.Setup(r => r.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.NotFound(query.Id));
    }
}
