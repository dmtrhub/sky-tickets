using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.GetAll;

public sealed class GetAllUsersQueryHandler(IApplicationDbContext context) 
    : IQueryHandler<GetAllUsersQuery, List<UserResponse>>
{
    public async Task<Result<List<UserResponse>>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await context.Users
            .Include(u => u.Reviews)
            .Include(u => u.Reservations)
            .ThenInclude(r => r.Flight)
            .ThenInclude(f => f.Airline)
            .Select(u => u.ToUserResponse())
            .ToListAsync(cancellationToken);

        if (users is null)
            return Result.Failure<List<UserResponse>>(UserErrors.NoUsersFound);

        return users;
    }
}