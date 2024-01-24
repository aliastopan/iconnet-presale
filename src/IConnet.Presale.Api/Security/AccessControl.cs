using IConnet.Presale.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace IConnet.Presale.Api.Security;

public static class AccessControl
{
    public static IServiceCollection AddSecurityTokenAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var securityTokenValidator = serviceProvider.GetRequiredService<ISecurityTokenValidatorService>();
                options.TokenValidationParameters = securityTokenValidator.GetAccessTokenValidationParameters();
            }

            options.MapInboundClaims = false;
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["access-token"];
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }

    public static IServiceCollection AddSecurityTokenAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy(Policies.AllowAnonymous, policy => policy.RequireAssertion(_ => true));
            options.AddPolicy(Policies.AdministratorPrivilege, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("privileges", "Administrator");
            });
        });

        return services;
    }
}
