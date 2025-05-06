using Application.Airlines.Create;
using FluentAssertions;

namespace Application.UnitTests.Airlines.Create;

public class CreateAirlineCommandValidatorTests
{
    private readonly CreateAirlineCommandValidator _validator = new();

    [Fact]
    public void Should_Pass_Validation_When_Command_Is_Valid()
    {
        // Arrange
        var command = new CreateAirlineCommand(
            "AirSerbia",
            "Bulevar Nikole Tesle 3, Beograd",
            "contact@airserbia.rs");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_Validation_When_Fields_Are_Empty()
    {
        // Arrange
        var command = new CreateAirlineCommand(
            "",
            "",
            "");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
        result.Errors.Should().Contain(e => e.PropertyName == "Address");
        result.Errors.Should().Contain(e => e.PropertyName == "ContactInfo");
    }

    [Fact]
    public void Should_Fail_Validation_When_Fields_Exceed_Max_Length()
    {
        // Arrange
        var command = new CreateAirlineCommand(
            new string('A', 21),          // > 20
            new string('B', 51),       // > 50
            new string('C', 51));    // > 50

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name" && e.ErrorMessage.Contains("must not exceed"));
        result.Errors.Should().Contain(e => e.PropertyName == "Address" && e.ErrorMessage.Contains("must not exceed"));
        result.Errors.Should().Contain(e => e.PropertyName == "ContactInfo" && e.ErrorMessage.Contains("must not exceed"));
    }
}
