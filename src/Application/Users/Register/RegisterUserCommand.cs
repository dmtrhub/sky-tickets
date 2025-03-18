using Application.Abstractions.Messaging;
using Domain;

namespace Application.Users.Register;

public sealed record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    DateTime DateOfBirth,
    Gender Gender
) : ICommand<Guid>;