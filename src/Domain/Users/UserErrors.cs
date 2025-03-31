using SharedKernel;

namespace Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Guid userId) =>
        Error.NotFound("Users.NotFound", $"The user with the Id = '{userId}' was not found");

    public static Error EmailInUse(string email) =>
        Error.Conflict("Users.EmailInUse", $"The email '{email}' is already in use");

    public static readonly Error NotFoundByEmail =
        Error.NotFound("Users.NotFoundByEmail", "The user with the specified email was not found");

    public static readonly Error InvalidCredentials =
        Error.Problem("Users.InvalidCredentials", "Email or password is incorrect");

    public static readonly Error NoUsersFound =
        Error.NotFound("Users.NoUsersFound", "No users found in the system");

    public static readonly Error Unauthenticated =
        Error.NotFound("Users.Unauthenticated", "Unauthenticated");
}
