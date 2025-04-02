using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.SearchUsers;

public sealed class SearchUsersQueryHandler(IApplicationDbContext context) 
    : IQueryHandler<SearchUsersQuery, List<UserResponse>>
{
    public async Task<Result<List<UserResponse>>> Handle(SearchUsersQuery query, CancellationToken cancellationToken)
    {
        var usersQuery = context.Users.AsQueryable();

        usersQuery = usersQuery.SearchUsers(query);       

        var users = await usersQuery
            .Select(u => u.ToUserResponse())
            .ToListAsync(cancellationToken);

        if (users.Count == 0)
            return Result.Failure<List<UserResponse>>(UserErrors.NoUsersFound);

        return users;
    }
}

