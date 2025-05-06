using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Users;
using SharedKernel;

namespace Application.Users.Delete;

public sealed class DeleteUserCommandHandler(
    IRepository<User> userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(command.Id, cancellationToken);

        if (user is null)
            return Result.Failure<Guid>(UserErrors.NotFound(command.Id));

        user.Raise(new UserDeletedDomainEvent(user.Id));

        userRepository.Remove(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}