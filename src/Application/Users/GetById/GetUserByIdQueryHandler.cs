using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.GetById;

public sealed class GetUserByIdQueryHandler(
    IApplicationDbContext context)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Where(u => u.Id == query.UserId)
            .Select(u => u.ToUserResponse())
            .SingleOrDefaultAsync(cancellationToken);

        if(user is null)
            return Result.Failure<UserResponse>(UserErrors.NotFound(query.UserId));

        return user;
    }
}