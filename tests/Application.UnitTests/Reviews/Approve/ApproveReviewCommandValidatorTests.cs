using Application.Reviews.Approve;
using FluentAssertions;

namespace Application.UnitTests.Reviews.Approve;

public class ApproveReviewCommandValidatorTests
{
    private readonly ApproveReviewCommandValidator _validator = new();

    [Fact]
    public void Should_Pass_Validation_When_Command_Is_Valid()
    {
        // Arrange
        var command = new ApproveReviewCommand(Guid.NewGuid(), true);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Id_Is_Empty()
    {
        // Arrange
        var command = new ApproveReviewCommand(Guid.Empty, true);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Id");
    }
}
