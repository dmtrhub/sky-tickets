﻿using Application.Reservations;
using Application.Reviews;

namespace Application.Users;

public sealed record UserResponse
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public DateOnly DateOfBirth { get; init; }
    public string Gender { get; init; }
    public string Role { get; init; }

    // Reservation
    public List<ReservationResponse> Reservations { get; init; }
    public List<ReviewResponse> Reviews { get; init; }
}
