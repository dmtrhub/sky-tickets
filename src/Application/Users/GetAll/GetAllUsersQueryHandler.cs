using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.GetAll;

public sealed class GetAllUsersQueryHandler(IApplicationDbContext context) : IQueryHandler<GetAllUsersQuery, List<UserResponse>>
{
    public async Task<Result<List<UserResponse>>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await context.Users
            .Select(u => new UserResponse
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                DateOfBirth = u.DateOfBirth,
                Gender = u.Gender.ToString(),
                Role = u.Role.ToString()
            })
            .ToListAsync(cancellationToken);

        if (users is null)
            return Result.Failure<List<UserResponse>>(UserErrors.NoUsersFound);

        return users;
    }
}