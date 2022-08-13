using System;
using CorePGIdentityTest.Models;
using CorePGIdentityTest.Security;
using CorePGIdentityTest.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace CorePGIdentityTest.Extensions
{
    public static class AuthSetup
    {
        public static IServiceCollection AddFirebaseAuth(this IServiceCollection services, JwtOptions jwtOptions)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.GetApplicationDefault()
            });

            // firebase auth
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(opt =>
             {

                 opt.Authority = jwtOptions.ValidIssuer;
                 opt.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = jwtOptions.ValidIssuer,
                     ValidAudience = jwtOptions.ValidAudience
                 };
                 opt.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = (ctx) =>
                     {
                         return Task.CompletedTask;
                     },
                     OnChallenge = (ctx) =>
                     {
                         return Task.CompletedTask;
                     },
                     OnTokenValidated = async (ctx) =>
                     {
                         if (!ctx.Principal.Identity.IsAuthenticated)
                         {
                             //logger.Warning($"OnTokenValidated invoked with unauthenticated user");
                             throw new InvalidOperationException("this shouldn't happen");
                         }
                         var userService = ctx.HttpContext.RequestServices.GetRequiredService<UserService>();

                         var name = ctx.Principal.Claims.First(c => c.Type == "user_id").Value;
                         var newUser = await userService.GetOrCreateUser(name);
                     }
                 };

             });


            services.AddAuthorization(options =>
            {
                // User must be authenticated
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                // can read forecasts  in their orgs
                options.AddPolicy(Policies.SubscriptionPremium, policy =>
                    policy.Requirements.Add(new HasScopeRequirement(Permissions.Read, jwtOptions.ValidIssuer))
                );

            });

            return services;
        }
    }
}

