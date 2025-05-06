using Application.Abstractions.Authentication;
using Application.Reservations;
using Application.Reviews;
using Application.Users.Register;
using Application.Users.SearchUsers;
using Application.Users.Update;
using Application.Users.UpdateMyProfile;
using Domain;
using Domain.Users;
using System.Globalization;

namespace Application.Users;

public static class UserMapExtensions
{
    public static UserResponse ToUserResponse(this User user) =>
        new()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            Gender = user.Gender.ToString(),
            Role = user.Role.ToString(),
            Reservations = user.Reservations.Select(r => r.ToReservationResponse()).ToList(),
            Reviews = user.Reviews.Select(r => r.ToReviewResponse()).ToList()
        };

    public static User ToUser(this RegisterUserCommand command, IPasswordHasher passwordHasher) =>
        User.Create(
            command.FirstName,
            command.LastName,
            command.Email,
            passwordHasher.Hash(command.Password),
            DateOnly.ParseExact(command.DateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Enum.Parse<Gender>(command.Gender, true));

    public static void UpdateUser(this User user, UpdateUserCommand command, IPasswordHasher passwordHasher)
    {
        if (!string.IsNullOrEmpty(command.FirstName))
            user.FirstName = command.FirstName;

        if (!string.IsNullOrEmpty(command.LastName))
            user.LastName = command.LastName;

        if (!string.IsNullOrEmpty(command.Email))
            user.Email = command.Email;

        if (!string.IsNullOrEmpty(command.Password))
            user.PasswordHash = passwordHasher.Hash(command.Password);

        if (!string.IsNullOrEmpty(command.DateOfBirth))
            user.DateOfBirth = DateOnly.ParseExact(command.DateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        if (!string.IsNullOrEmpty(command.Gender))
            user.Gender = Enum.Parse<Gender>(command.Gender, true);

        if (!string.IsNullOrEmpty(command.Role))
            user.Role = Enum.Parse<UserRole>(command.Role, true);
    }

    public static void UpdateMyProfile(this User user, UpdateMyProfileCommand command, IPasswordHasher passwordHasher)
    {
        if (!string.IsNullOrEmpty(command.FirstName))
            user.FirstName = command.FirstName;

        if (!string.IsNullOrEmpty(command.LastName))
            user.LastName = command.LastName;

        if (!string.IsNullOrEmpty(command.Email))
            user.Email = command.Email;

        if (!string.IsNullOrEmpty(command.Password))
            user.PasswordHash = passwordHasher.Hash(command.Password);

        if (!string.IsNullOrEmpty(command.DateOfBirth))
            user.DateOfBirth = DateOnly.ParseExact(command.DateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        if (!string.IsNullOrEmpty(command.Gender))
            user.Gender = Enum.Parse<Gender>(command.Gender, true);
    }

    public static IQueryable<User> SearchUsers(this IQueryable<User> users, SearchUsersQuery query)
    {
        if (!string.IsNullOrWhiteSpace(query.FirstName))
            users = users.Where(u => u.FirstName.Contains(query.FirstName));

        if (!string.IsNullOrWhiteSpace(query.LastName))
            users = users.Where(u => u.LastName.Contains(query.LastName));

        if (query.DateOfBirthFrom.HasValue)
            users = users.Where(u => u.DateOfBirth >= query.DateOfBirthFrom.Value);

        if (query.DateOfBirthTo.HasValue)
            users = users.Where(u => u.DateOfBirth <= query.DateOfBirthTo.Value);

        return users;
    }
}