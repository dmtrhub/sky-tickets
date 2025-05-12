using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Application.Reviews.Delete;
using Application.UnitTests.Builders;
using Domain.Reviews;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Reviews.Delete;

public class DeleteReviewCommandHandlerTests
{
    private readonly Mock<IRepository<Review>> _reviewRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    private readonly DeleteReviewCommandHandler _handler;

    public DeleteReviewCommandHandlerTests()
    {
        _handler = new DeleteReviewCommandHandler(
            _reviewRepository.Object,
            _unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ReviewExists_DeletesReview()
    {
        // Arrange
        var review = new ReviewBuilder().Build();

        var command = new DeleteReviewCommand(review.Id);

        _reviewRepository.Setup(x => x.GetByIdAsync(review.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(review);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Value.Should().Be(review.Id);
        result.IsSuccess.Should().BeTrue();

        _reviewRepository.Verify(x => x.Remove(review), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ReviewDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var command = new DeleteReviewCommand(Guid.NewGuid());

        _reviewRepository.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Review?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.NotFound(command.Id));

        _reviewRepository.Verify(x => x.Remove(It.IsAny<Review>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
