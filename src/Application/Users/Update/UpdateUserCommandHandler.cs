using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

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
            return Result.Failure<Guid>(UserErrors.EmailInUse(command.Email!));

        user.UpdateUser(command, passwordHasher);

        user.Raise(new UserUpdatedDomainEvent(user.Id));

        await context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}