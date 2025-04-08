using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.GetById;

public sealed class GetUserByIdQueryHandler(IRepository<User> userRepository)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var userQuery = await userRepository.AsQueryable();

        var user = await userQuery
            .Where(u => u.Id == query.UserId)
            .Include(u => u.Reviews)
            .Include(u => u.Reservations)
            .ThenInclude(r => r.Flight)
            .ThenInclude(f => f.Airline)
            .Select(u => u.ToUserResponse())
            .FirstOrDefaultAsync(cancellationToken);

        if(user is null)
            return Result.Failure<UserResponse>(UserErrors.NotFound(query.UserId));

        return user;
    }
}