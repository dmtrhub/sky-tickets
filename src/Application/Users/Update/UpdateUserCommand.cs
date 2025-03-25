using Application.Abstractions.Messaging;

namespace Application.Users.Update;

public sealed record UpdateUserCommand(
    Guid Id,
    string? FirstName,
    string? LastName,
    string? Email,
    string? Password,
    string? DateOfBirth,
    string? Gender,
    string? Role) : ICommand<Guid>;