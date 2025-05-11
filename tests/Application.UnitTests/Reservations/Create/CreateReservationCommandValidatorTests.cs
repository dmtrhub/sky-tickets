using Application.Reservations.Create;
using FluentAssertions;

namespace Application.UnitTests.Reservations.Create;

public class CreateReservationCommandValidatorTests
{
    private readonly CreateReservationCommandValidator _validator = new();

    [Fact]
    public void Should_Pass_Validation_When_Command_Is_Valid()
    {
        // Arrange
        var command = new CreateReservationCommand(
            Guid.NewGuid(),
            1);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_FlightId_Is_Empty()
    {
        // Arrange
        var command = new CreateReservationCommand(
            Guid.Empty,
            1);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FlightId");
    }

    [Fact]
    public void Should_Fail_When_PassengerCount_Is_Zero()
    {
        // Arrange
        var command = new CreateReservationCommand(
            Guid.NewGuid(),
            0);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PassengerCount");
    }
}
