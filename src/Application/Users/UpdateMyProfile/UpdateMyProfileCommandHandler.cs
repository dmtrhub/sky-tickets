using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Extensions;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using SharedKernel;

namespace Application.Users.UpdateMyProfile;

public sealed class UpdateMyProfileCommandHandler(
    IRepository<User> userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IHttpContextAccessor httpContextAccessor) : ICommandHandler<UpdateMyProfileCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateMyProfileCommand command, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.GetUserId();

        if (userId is null)
            return Result.Failure<Guid>(UserErrors.Unauthenticated);

        var user = await userRepository.GetByIdAsync(userId.Value, cancellationToken);

        if (user is null)
            return Result.Failure<Guid>(UserErrors.NotFound(userId.Value));

        if (await userRepository.AnyAsync(u => u.Email == command.Email && u.Id != userId, cancellationToken))
            return Result.Failure<Guid>(UserErrors.EmailInUse(command.Email!));

        user.UpdateMyProfile(command, passwordHasher);
        user.Raise(new UserUpdatedDomainEvent(user.Id));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}