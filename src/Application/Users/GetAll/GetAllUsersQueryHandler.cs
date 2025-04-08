using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.GetAll;

public sealed class GetAllUsersQueryHandler(IRepository<User> userRepository) 
    : IQueryHandler<GetAllUsersQuery, List<UserResponse>>
{
    public async Task<Result<List<UserResponse>>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        var userQuery = await userRepository.AsQueryable();

        var users = await userQuery
            .Include(u => u.Reviews)
            .Include(u => u.Reservations)
            .ThenInclude(r => r.Flight)
            .ThenInclude(f => f.Airline)
            .Select(u => u.ToUserResponse())
            .ToListAsync(cancellationToken);

        if (users.Count == 0)
            return Result.Failure<List<UserResponse>>(UserErrors.NoUsersFound);

        return users;
    }
}