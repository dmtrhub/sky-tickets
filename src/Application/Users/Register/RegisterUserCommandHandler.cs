﻿using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.Register;

public class RegisterUserCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher) : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        if(await context.Users.AnyAsync(u => u.Email == command.Email, cancellationToken))
            return Result.Failure<Guid>(UserErrors.EmailInUse(command.Email));

        var user = command.ToUser(passwordHasher);

        user.Raise(new UserRegisteredDomainEvent(user.Id));

        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}