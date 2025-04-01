using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Update;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Security.Claims;

namespace Application.Users.UpdateMyProfile;

public sealed record UpdateMyProfileCommand(
    string? FirstName,
    string? LastName,
    string? Email,
    string? Password,
    string? DateOfBirth,
    string? Gender) : ICommand<Guid>;

public sealed class UpdateMyProfileCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    IHttpContextAccessor httpContextAccessor) : ICommandHandler<UpdateUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            return Result.Failure<Guid>(UserErrors.Unauthenticated);

        var user = await context.Users.FindAsync(parsedUserId, cancellationToken);

        if (user is null)
            return Result.Failure<Guid>(UserErrors.NotFound(command.Id));

        if (await context.Users.AnyAsync(u => u.Email == command.Email && u.Id != command.Id, cancellationToken))
            return Result.Failure<Guid>(UserErrors.EmailInUse(command.Email!));

        user.UpdateUser(command, passwordHasher, false);

        user.Raise(new UserUpdatedDomainEvent(user.Id));

        await context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}