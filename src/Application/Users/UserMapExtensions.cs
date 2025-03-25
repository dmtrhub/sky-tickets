using Application.Abstractions.Authentication;
using Application.Users.Register;
using Application.Users.Update;
using Domain;
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
            Role = user.Role.ToString()
        };

    public static User ToUser(this RegisterUserCommand command, IPasswordHasher passwordHasher) =>
        new()
        {
            Id = Guid.NewGuid(),
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            PasswordHash = passwordHasher.Hash(command.Password),
            DateOfBirth = DateOnly.ParseExact(command.DateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Gender = Enum.Parse<Gender>(command.Gender, true),
            Role = UserRole.Passenger,
            Reservations = [],
            Reviews = []
        };

    public static void UpdateUser(this User user, UpdateUserCommand command, IPasswordHasher passwordHasher)
    {
        if(!string.IsNullOrEmpty(command.FirstName))
            user.FirstName = command.FirstName;

        if(!string.IsNullOrEmpty(command.LastName))
            user.LastName = command.LastName;

        if (!string.IsNullOrEmpty(command.Email))
            user.Email = command.Email;

        if(!string.IsNullOrEmpty(command.Password))
            user.PasswordHash = passwordHasher.Hash(command.Password);

        if (!string.IsNullOrEmpty(command.DateOfBirth))
            user.DateOfBirth = DateOnly.ParseExact(command.DateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        if (!string.IsNullOrEmpty(command.Gender))
            user.Gender = Enum.Parse<Gender>(command.Gender, true);

        if (!string.IsNullOrEmpty(command.Role))
            user.Role = Enum.Parse<UserRole>(command.Role, true);
    }
}