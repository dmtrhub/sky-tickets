using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Extensions;
using Application.Users.Update;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Security.Claims;

namespace Application.Users.UpdateMyProfile;

public sealed class UpdateMyProfileCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    IHttpContextAccessor httpContextAccessor) : ICommandHandler<UpdateMyProfileCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateMyProfileCommand command, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.GetUserId();

        if (userId is null)
            return Result.Failure<Guid>(UserErrors.Unauthenticated);

        var user = await context.Users.FindAsync(userId, cancellationToken);

        if (user is null)
            return Result.Failure<Guid>(UserErrors.NotFound(userId.Value));

        if (await context.Users.AnyAsync(u => u.Email == command.Email && u.Id != userId, cancellationToken))
            return Result.Failure<Guid>(UserErrors.EmailInUse(command.Email!));

        user.UpdateMyProfile(command, passwordHasher);

        user.Raise(new UserUpdatedDomainEvent(user.Id));

        await context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}