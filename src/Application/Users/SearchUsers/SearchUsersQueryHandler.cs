using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.SearchUsers;

public sealed class SearchUsersQueryHandler(IRepository<User> userRepository) 
    : IQueryHandler<SearchUsersQuery, List<UserResponse>>
{
    public async Task<Result<List<UserResponse>>> Handle(SearchUsersQuery query, CancellationToken cancellationToken)
    {
        var userQuery = await userRepository.AsQueryable();

        userQuery = userQuery.SearchUsers(query);       

        var users = await userQuery
            .Select(u => u.ToUserResponse())
            .ToListAsync(cancellationToken);

        if (users.Count == 0)
            return Result.Failure<List<UserResponse>>(UserErrors.NoUsersFound);

        return users;
    }
}

