using Application.Airlines.Update;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Airlines.Update;

public class UpdateAirlineCommandValidatorTests
{
    private readonly UpdateAirlineCommandValidator _validator = new();

    [Fact]
    public void Should_Pass_When_ValidDataProvided()
    {
        // Arrange
        var command = new UpdateAirlineCommand(
            Guid.NewGuid(),
            "AirSerbia",
            "Nikola Tesla Airport",
            "info@airserbia.rs"
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_NameIsTooLong()
    {
        var command = new UpdateAirlineCommand(
            Guid.NewGuid(),
            new string('A', 21),
            null,
            null
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public void Should_NotValidateEmptyStrings()
    {
        var command = new UpdateAirlineCommand(
            Guid.NewGuid(),
            "",
            "     ", // whitespace
            null
        );

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Name);
        result.ShouldNotHaveValidationErrorFor(c => c.Address);
        result.ShouldNotHaveValidationErrorFor(c => c.ContactInfo);
    }

    [Fact]
    public void Should_Fail_When_AddressOrContactInfoTooLong()
    {
        var command = new UpdateAirlineCommand(
            Guid.NewGuid(),
            "ValidName",
            new string('B', 51),
            new string('C', 51)
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Address);
        result.ShouldHaveValidationErrorFor(c => c.ContactInfo);
    }
}
