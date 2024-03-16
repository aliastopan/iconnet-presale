using IConnet.Presale.Domain.Enums;
using IConnet.Presale.Infrastructure.Security;

namespace IConnet.Presale.WebApp.Security;

public static class AccessControl
{
    public static IServiceCollection AddAccessControl(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.DeveloperAccess, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireAssertion(context =>
                {
                    return context.User.HasClaim(c => c.Type == JwtClaimTypes.Role && c.Value == UserRole.SuperUser.ToString())
                        && context.User.HasClaim(c => c.Type == JwtClaimTypes.JobTitle && string.Equals(c.Value, "Developer", StringComparison.OrdinalIgnoreCase));
                });
            });
            options.AddPolicy(Policies.AdministratorPrivilege, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(JwtClaimTypes.Privileges, UserPrivilege.Administrator.ToString());
            });
            options.AddPolicy(Policies.RoleHelpdesk, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireAssertion(context =>
                {
                    return context.User.HasClaim(c => c.Type == JwtClaimTypes.Role && c.Value == UserRole.Helpdesk.ToString())
                        || context.User.HasClaim(c => c.Type == JwtClaimTypes.Privileges && c.Value == UserPrivilege.Administrator.ToString());
                });
            });
            options.AddPolicy(Policies.RolePlanningAssetCoverage, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireAssertion(context =>
                {
                    return context.User.HasClaim(c => c.Type == JwtClaimTypes.Role && c.Value == UserRole.PlanningAssetCoverage.ToString())
                        || context.User.HasClaim(c => c.Type == JwtClaimTypes.Privileges && c.Value == UserPrivilege.Administrator.ToString());
                });
            });
        });

        return services;
    }
}
