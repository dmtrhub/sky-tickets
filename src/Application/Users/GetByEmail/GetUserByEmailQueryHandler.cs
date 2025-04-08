using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.GetByEmail;

public sealed class GetUserByEmailQueryHandler(IRepository<User> userRepository) 
    : IQueryHandler<GetUserByEmailQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken)
    {
        var userQuery = await userRepository.AsQueryable();

        var user = await userQuery
            .Where(u => u.Email == query.Email)
            .Include(u => u.Reviews)
            .Include(u => u.Reservations)
            .ThenInclude(r => r.Flight)
            .ThenInclude(f => f.Airline)
            .Select(u => u.ToUserResponse())
            .FirstOrDefaultAsync(cancellationToken);

        if(user is null)
            return Result.Failure<UserResponse>(UserErrors.NotFoundByEmail);

        return user;
    }
}