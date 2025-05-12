using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Application.Reviews.Approve;
using Application.UnitTests.Builders;
using Domain;
using Domain.Reviews;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Reviews.Approve;

public class ApproveReviewCommandHandlerTests
{
    private readonly Mock<IRepository<Review>> _reviewRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    private readonly ApproveReviewCommandHandler _handler;

    public ApproveReviewCommandHandlerTests()
    {
        _handler = new ApproveReviewCommandHandler(
            _reviewRepository.Object, 
            _unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ReviewNotFound_ReturnsFailure()
    {
        // Arrange
        var command = new ApproveReviewCommand(Guid.NewGuid(), true);

        _reviewRepository.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Review?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.NotFound(command.Id));
    }

    [Fact]
    public async Task Handle_ReviewNotCreated_ReturnsFailure()
    {
        // Arrange
        var review = new ReviewBuilder()
            .WithStatus(Domain.ReviewStatus.Rejected)
            .Build();

        var command = new ApproveReviewCommand(review.Id, true);

        _reviewRepository.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(review);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.NotCreated);
    }

    [Fact]
    public async Task Handle_ReviewApproved_SavesChanges()
    {
        // Arrange
        var review = new ReviewBuilder()
            .WithStatus(Domain.ReviewStatus.Created)
            .Build();

        var command = new ApproveReviewCommand(review.Id, true);

        _reviewRepository.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(review);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(review.Id);
        review.Status.Should().Be(ReviewStatus.Approved);

        _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
