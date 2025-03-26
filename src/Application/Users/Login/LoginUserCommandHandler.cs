using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.Login;

public class LoginUserCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider) : ICommandHandler<LoginUserCommand, string>
{
    public async Task<Result<string>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email == command.Email, cancellationToken);

        if(user is null) 
            return Result.Failure<string>(UserErrors.NotFoundByEmail);

        var verified = passwordHasher.Verify(command.Password, user.PasswordHash);

        if(!verified)
            return Result.Failure<string>(UserErrors.InvalidCredentials);

        var token = tokenProvider.Create(user);

        return token;
    }
}