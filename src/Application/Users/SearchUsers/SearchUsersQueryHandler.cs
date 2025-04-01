using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.SearchUsers;

public sealed class SearchUsersQueryHandler
    (IApplicationDbContext context) : IQueryHandler<SearchUsersQuery, List<UserResponse>>
{
    public async Task<Result<List<UserResponse>>> Handle(SearchUsersQuery query, CancellationToken cancellationToken)
    {
        var usersQuery = context.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.FirstName))        
            usersQuery = usersQuery.Where(u => u.FirstName.Contains(query.FirstName));
        

        if (!string.IsNullOrWhiteSpace(query.LastName))      
            usersQuery = usersQuery.Where(u => u.LastName.Contains(query.LastName));
        

        if (query.DateOfBirthFrom.HasValue)
            usersQuery = usersQuery.Where(u => u.DateOfBirth >= query.DateOfBirthFrom.Value);

        if (query.DateOfBirthTo.HasValue)
            usersQuery = usersQuery.Where(u => u.DateOfBirth <= query.DateOfBirthTo.Value);

        var users = await usersQuery
            .Select(u => u.ToUserResponse())
            .ToListAsync(cancellationToken);

        return users;
    }
}

