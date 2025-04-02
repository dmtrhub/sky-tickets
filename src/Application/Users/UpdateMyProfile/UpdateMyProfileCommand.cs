using Application.Abstractions.Messaging;

namespace Application.Users.UpdateMyProfile;

public sealed record UpdateMyProfileCommand(
    string? FirstName,
    string? LastName,
    string? Email,
    string? Password,
    string? DateOfBirth,
    string? Gender) : ICommand<Guid>;
