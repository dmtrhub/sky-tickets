using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Users;
using SharedKernel;

namespace Application.Users.Update;

public sealed class UpdateUserCommandHandler(
    IRepository<User> userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher) : ICommandHandler<UpdateUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(command.Id, cancellationToken);

        if (user is null)
            return Result.Failure<Guid>(UserErrors.NotFound(command.Id));

        if (await userRepository.AnyAsync(u => u.Email == command.Email && u.Id != command.Id, cancellationToken))
            return Result.Failure<Guid>(UserErrors.EmailInUse(command.Email!));

        user.UpdateUser(command, passwordHasher);
        user.Raise(new UserUpdatedDomainEvent(user.Id));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}