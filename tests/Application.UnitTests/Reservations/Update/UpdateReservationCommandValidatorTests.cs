using Application.Reservations.Update;
using Domain;
using FluentAssertions;

namespace Application.UnitTests.Reservations.Update;

public class UpdateReservationCommandValidatorTests
{
    private readonly UpdateReservationCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldFail_WhenPassengerCountIsZero()
    {
        // Arrange
        var command = new UpdateReservationCommand(Guid.NewGuid(), 0, "Active");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "PassengerCount");
    }

    [Fact]
    public void Validate_ShouldFail_WhenStatusIsInvalid()
    {
        // Arrange
        var command = new UpdateReservationCommand(Guid.NewGuid(), 1, "WrongStatus");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Status");
    }

    [Fact]
    public void Validate_ShouldSucceed_WhenAllFieldsAreValid()
    {
        // Arrange
        var command = new UpdateReservationCommand(Guid.NewGuid(), 2, ReservationStatus.Canceled.ToString());

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_ShouldSucceed_WhenOptionalFieldsAreNull()
    {
        // Arrange
        var command = new UpdateReservationCommand(Guid.NewGuid(), null, null);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

}
