using Application.Reviews.Create;
using FluentAssertions;

namespace Application.UnitTests.Reviews.Create;

public class CreateReviewCommandValidatorTests
{
    private readonly CreateReviewCommandValidator _validator = new();

    [Fact]
    public void Should_Pass_Validation_When_Command_Is_Valid()
    {
        // Arrange
        var command = new CreateReviewCommand(
            Guid.NewGuid(),
            "Great flight!",
            "Everything was smooth and on time.",
            null);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_AirlineId_Is_Empty()
    {
        // Arrange
        var command = new CreateReviewCommand(
            Guid.Empty,
            "Title",
            "Content",
            null);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "AirlineId");
    }

    [Fact]
    public void Should_Fail_When_Title_Is_Empty()
    {
        // Arrange
        var command = new CreateReviewCommand(
            Guid.NewGuid(),
            "",
            "Valid content",
            null);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }

    [Fact]
    public void Should_Fail_When_Title_Too_Long()
    {
        // Arrange
        var command = new CreateReviewCommand(
            Guid.NewGuid(),
            new string('A', 51),
            "Valid content",
            null);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }

    [Fact]
    public void Should_Fail_When_Content_Is_Empty()
    {
        // Arrange
        var command = new CreateReviewCommand(
            Guid.NewGuid(),
            "Valid Title",
            "",
            null);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Content");
    }

    [Fact]
    public void Should_Fail_When_Content_Too_Long()
    {
        // Arrange
        var command = new CreateReviewCommand(
            Guid.NewGuid(),
            "Valid Title",
            new string('B', 501),
            null);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Content");
    }
}

