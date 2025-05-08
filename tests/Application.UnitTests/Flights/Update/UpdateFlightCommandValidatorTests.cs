using Application.Flights.Update;
using FluentAssertions;

namespace Application.UnitTests.Flights.Update;

public class UpdateFlightCommandValidatorTests
{
    private readonly UpdateFlightCommandValidator _validator = new();

    [Fact]
    public void Should_HaveError_When_IdIsEmpty()
    {
        // Arrange
        var command = new UpdateFlightCommand(Guid.Empty, null, null, null, null, null, null);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.Errors.Should().ContainSingle(x => x.PropertyName == "Id");
    }

    [Fact]
    public void Should_HaveError_When_DepartureTime_IsInvalid()
    {
        // Arrange
        var command = new UpdateFlightCommand(Guid.NewGuid(), "not-a-date", null, null, null, null, null);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.Errors.Should().ContainSingle(x => x.PropertyName == "DepartureTime");
    }

    [Fact]
    public void Should_HaveError_When_ArrivalTime_IsInvalid()
    {
        // Arrange
        var command = new UpdateFlightCommand(Guid.NewGuid(), null, "invalid-date", null, null, null, null);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.Errors.Should().ContainSingle(x => x.PropertyName == "ArrivalTime");
    }

    [Fact]
    public void Should_HaveError_When_ArrivalTime_IsBeforeDepartureTime()
    {
        // Arrange
        var departure = DateTime.UtcNow.AddHours(2).ToString("O");
        var arrival = DateTime.UtcNow.ToString("O");

        var command = new UpdateFlightCommand(Guid.NewGuid(), departure, arrival, null, null, null, null);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.Errors.Should().Contain(x => x.PropertyName == "ArrivalTime");
    }

    [Fact]
    public void Should_HaveError_When_AvailableSeats_IsNegative()
    {
        // Arrange
        var command = new UpdateFlightCommand(Guid.NewGuid(), null, null, -1, null, null, null);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.Errors.Should().ContainSingle(x => x.PropertyName == "AvailableSeats");
    }

    [Fact]
    public void Should_HaveError_When_BookedSeats_IsNegative()
    {
        // Arrange
        var command = new UpdateFlightCommand(Guid.NewGuid(), null, null, 100, -5, null, null);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.Errors.Should().ContainSingle(x => x.PropertyName == "BookedSeats");
    }

    [Fact]
    public void Should_HaveError_When_Price_IsNegative()
    {
        // Arrange
        var command = new UpdateFlightCommand(Guid.NewGuid(), null, null, null, null, -50, null);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.Errors.Should().ContainSingle(x => x.PropertyName == "Price");
    }

    [Fact]
    public void Should_HaveError_When_Status_IsInvalid()
    {
        // Arrange
        var command = new UpdateFlightCommand(Guid.NewGuid(), null, null, null, null, null, "InvalidStatus");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.Errors.Should().ContainSingle(x => x.PropertyName == "Status");
    }

    [Fact]
    public void Should_NotHaveError_When_AllValuesAreValid()
    {
        // Arrange
        var departure = DateTime.UtcNow.AddMinutes(5).ToString("yyyy-MM-dd HH:mm");
        var arrival = DateTime.UtcNow.AddHours(2).ToString("yyyy-MM-dd HH:mm");

        var command = new UpdateFlightCommand(Guid.NewGuid(), departure, arrival, 100, 50, 199.99m, "Completed");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
