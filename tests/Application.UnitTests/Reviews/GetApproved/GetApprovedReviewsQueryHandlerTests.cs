using Application.Abstractions.Repositories;
using Application.Reviews.GetApproved;
using Application.UnitTests.Builders;
using Domain;
using Domain.Reviews;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Reviews.GetApproved;

public class GetApprovedReviewsQueryHandlerTests
{
    private readonly Mock<IRepository<Review>> _reviewRepositoryMock = new();

    private readonly GetApprovedReviewsQueryHandler _handler;
    
    public GetApprovedReviewsQueryHandlerTests()
    {
        _handler = new GetApprovedReviewsQueryHandler(
            _reviewRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Approved_Reviews()
    {
        // Arrange
        var mockDbSet = new List<Review>
        {
            new ReviewBuilder().Build(),
            new ReviewBuilder().WithStatus(ReviewStatus.Approved).Build(),
            new ReviewBuilder().WithStatus(ReviewStatus.Approved).Build()
        }
        .AsQueryable().BuildMockDbSet();

        _reviewRepositoryMock.Setup(repo => repo.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        // Act
        var result = await _handler.Handle(new GetApprovedReviewsQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_Should_Return_No_Reviews_Found_Error()
    {
        // Arrange
        var mockDbSet = new List<Review>().AsQueryable().BuildMockDbSet();

        _reviewRepositoryMock.Setup(repo => repo.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        // Act
        var result = await _handler.Handle(new GetApprovedReviewsQuery(), CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.NoReviewsFound);
    }
}
