using Application.Flights.Create;
using FluentAssertions;

namespace Application.UnitTests.Flights.Create;

public class CreateFlightCommandValidatorTests
{
    private readonly CreateFlightCommandValidator _validator = new();

    [Fact]
    public void Should_Pass_Validation_When_Command_Is_Valid()
    {
        // Arrange
        var command = new CreateFlightCommand(
            Guid.NewGuid(),
            "Belgrade",
            "Paris",
            DateTime.UtcNow.AddHours(2).ToString("yyyy-MM-dd HH:mm"),
            DateTime.UtcNow.AddHours(5).ToString("yyyy-MM-dd HH:mm"),
            100,
            150);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Guid_Is_Empty()
    {
        var command = new CreateFlightCommand(
            Guid.Empty,
            "Belgrade",
            "Paris",
            DateTime.UtcNow.AddHours(2).ToString("yyyy-MM-dd HH:mm"),
            DateTime.UtcNow.AddHours(5).ToString("yyyy-MM-dd HH:mm"),
            100,
            150);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "AirlineId");
    }

    [Fact]
    public void Should_Fail_When_DepartureTime_Is_Not_Valid()
    {
        var command = new CreateFlightCommand(
            Guid.NewGuid(),
            "Belgrade",
            "Paris",
            "not-a-date",
            DateTime.UtcNow.AddHours(5).ToString("yyyy-MM-dd HH:mm"),
            100,
            150);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DepartureTime");
    }

    [Fact]
    public void Should_Fail_When_DepartureTime_Is_After_ArrivalTime()
    {
        var departure = DateTime.UtcNow.AddHours(5).ToString("yyyy-MM-dd HH:mm");
        var arrival = DateTime.UtcNow.AddHours(2).ToString("yyyy-MM-dd HH:mm");

        var command = new CreateFlightCommand(
            Guid.NewGuid(),
            "Belgrade",
            "Paris",
            departure,
            arrival,
            100,
            150);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DepartureTime" || e.PropertyName == "ArrivalTime");
    }

    [Fact]
    public void Should_Fail_When_Price_Or_Seats_Are_Invalid()
    {
        var command = new CreateFlightCommand(
            Guid.NewGuid(),
            "Belgrade",
            "Paris",
            DateTime.UtcNow.AddHours(2).ToString("yyyy-MM-dd HH:mm"),
            DateTime.UtcNow.AddHours(4).ToString("yyyy-MM-dd HH:mm"),
            0,
            0);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "AvailableSeats");
        result.Errors.Should().Contain(e => e.PropertyName == "Price");
    }
}
