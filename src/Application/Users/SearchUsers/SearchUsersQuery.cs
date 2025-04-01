using Application.Abstractions.Messaging;

namespace Application.Users.SearchUsers;

public sealed record SearchUsersQuery(
    string? FirstName,
    string? LastName,
    DateOnly? DateOfBirthFrom,
    DateOnly? DateOfBirthTo) : IQuery<List<UserResponse>>;
