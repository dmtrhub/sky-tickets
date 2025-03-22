using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization;

public static class AuthorizationPolicies
{
    public static AuthorizationPolicy AdministratorPolicy =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole("Administrator") // Admin only
            .Build();

    public static AuthorizationPolicy PassengerPolicy =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole("Passenger", "Administrator") //Multiple roles can be specified
            .Build();
}
