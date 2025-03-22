using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Globalization;

namespace Application.Users.Register;

public class RegisterUserCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher) : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        if(await context.Users.AnyAsync(u => u.Email == command.Email, cancellationToken))
            return Result.Failure<Guid>(UserErrors.EmailInUse(command.Email));

        var user = new User
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

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}