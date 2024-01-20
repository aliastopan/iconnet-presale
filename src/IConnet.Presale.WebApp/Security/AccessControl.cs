using IConnet.Presale.Domain.Enums;
using IConnet.Presale.Infrastructure.Security;

namespace IConnet.Presale.WebApp.Security;

public static class AccessControl
{
    public static IServiceCollection AddAccessControl(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.AdministratorPrivilege, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(JwtClaimTypes.Privileges, UserPrivilege.Administrator.ToString());
            });
            options.AddPolicy(Policies.RoleHelpdesk, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(JwtClaimTypes.Role, UserRole.Helpdesk.ToString());
            });
            options.AddPolicy(Policies.RolePlanningAssetCoverage, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(JwtClaimTypes.Role, UserRole.PlanningAssetCoverage.ToString());
            });
        });

        return services;
    }
}
