using Application.Reviews.Update;
using FluentAssertions;

namespace Application.UnitTests.Reviews.Update;

public class UpdateReviewCommandValidatorTests
{
    private readonly UpdateReviewCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldSucceed_WhenTitleIsEmpty()
    {
        // Arrange
        var command = new UpdateReviewCommand(Guid.NewGuid(), "", "Valid content.", "https://image.url");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_ShouldSucceed_WhenContentIsEmpty()
    {
        // Arrange
        var command = new UpdateReviewCommand(Guid.NewGuid(), "Valid title", "", "https://image.url");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_ShouldSucceed_WhenTitleAndContentAreEmpty()
    {
        // Arrange
        var command = new UpdateReviewCommand(Guid.NewGuid(), "", "", "https://image.url");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_ShouldFail_WhenTitleExceedsMaxLength()
    {
        // Arrange
        var command = new UpdateReviewCommand(Guid.NewGuid(), new string('A', 51), "Valid content.", "https://image.url");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_ShouldFail_WhenContentExceedsMaxLength()
    {
        // Arrange
        var command = new UpdateReviewCommand(Guid.NewGuid(), "Valid title", new string('A', 501), "https://image.url");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_ShouldSucceed_WhenTitleAndContentAreValid()
    {
        // Arrange
        var command = new UpdateReviewCommand(Guid.NewGuid(), "Valid title", "Valid content.", "https://image.url");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_ShouldSucceed_WhenOptionalFieldsAreNull()
    {
        // Arrange
        var command = new UpdateReviewCommand(Guid.NewGuid(), null, null, "https://image.url");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}

