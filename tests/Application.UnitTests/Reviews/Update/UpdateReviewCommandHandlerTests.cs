using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Application.Reviews.Update;
using Application.UnitTests.Builders;
using Domain.Reviews;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Reviews.Update;

public class UpdateReviewCommandHandlerTests
{
    private readonly Mock<IRepository<Review>> _reviewRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly UpdateReviewCommandHandler _handler;

    public UpdateReviewCommandHandlerTests()
    {
        _handler = new UpdateReviewCommandHandler(
            _reviewRepositoryMock.Object, 
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ReviewExists_UpdatesReviewAndSavesChanges()
    {
        // Arrange
        var review = new ReviewBuilder().Build();

        var command = new UpdateReviewCommand(review.Id, "Updated review","Updated review content", null);

        _reviewRepositoryMock
            .Setup(repo => repo.GetByIdAsync(review.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(review);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Value.Should().Be(review.Id);
        review.Content.Should().Be("Updated review content");

        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ReviewDoesNotExist_ReturnsNotFoundError()
    {
        // Arrange
        var command = new UpdateReviewCommand(Guid.NewGuid(), "Updated review", "Updated review content", null);

        _reviewRepositoryMock
            .Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Review?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.NotFound(command.Id));

        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
