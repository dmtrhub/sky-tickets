using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Globalization;

namespace Application.Users.Update;

public sealed class UpdateUserCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher) : ICommandHandler<UpdateUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await context.Users.FindAsync([command.Id], cancellationToken);

        if (user is null)
            return Result.Failure<Guid>(UserErrors.NotFound(command.Id));

        if (await context.Users.AnyAsync(u => u.Email == command.Email && u.Id != command.Id, cancellationToken))
            return Result.Failure<Guid>(UserErrors.EmailInUse(command.Email));

        user.FirstName = command.FirstName;
        user.LastName = command.LastName;
        user.Email = command.Email;
        user.PasswordHash = passwordHasher.Hash(command.Password);
        user.DateOfBirth = DateOnly.ParseExact(command.DateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        user.Gender = Enum.Parse<Gender>(command.Gender, true);
        user.Role = Enum.Parse<UserRole>(command.Role, true);

        await context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}