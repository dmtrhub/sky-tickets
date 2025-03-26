using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain;
using Domain.Users;
using SharedKernel;

namespace Application.Users.Delete;

public sealed class DeleteUserCommandHandler(IApplicationDbContext context) 
    : ICommandHandler<DeleteUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await context.Users.FindAsync([command.Id], cancellationToken);

        if (user is null)
            return Result.Failure<Guid>(UserErrors.NotFound(command.Id));

        user.Raise(new UserDeletedDomainEvent(user.Id));

        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}