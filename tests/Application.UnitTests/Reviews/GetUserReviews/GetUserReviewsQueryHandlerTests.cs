using Application.Abstractions.Repositories;
using Application.Reviews.Create;
using Application.Reviews.GetUserReviews;
using Application.UnitTests.Extensions;
using Domain.Reviews;
using Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MockQueryable.Moq;
using Moq;

namespace Application.UnitTests.Reviews.GetUserReviews;

public class GetUserReviewsQueryHandlerTests
{
    private readonly Mock<IRepository<Review>> _reviewRepositoryMock = new();
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();

    private readonly GetUserReviewsQueryHandler _handler;

    public GetUserReviewsQueryHandlerTests()
    {
        _handler = new GetUserReviewsQueryHandler(
            _reviewRepositoryMock.Object, 
            _httpContextAccessorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthenticated_WhenUserIdIsNull()
    {
        // Arrange
        _httpContextAccessorMock.SetUserId(null);

        var query = new GetUserReviewsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.Unauthenticated);
    }

    [Fact]
    public async Task Handle_ShouldReturnNoReviewsFound_WhenNoReviewsExist()
    {
        // Arrange
        _httpContextAccessorMock.SetUserId(Guid.NewGuid());

        var query = new GetUserReviewsQuery();

        var mockDbSet = new List<Review>().AsQueryable().BuildMockDbSet();

        _reviewRepositoryMock.Setup(x => x.AsQueryable())
            .ReturnsAsync(mockDbSet.Object);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.NoReviewsFound);
    }
}
