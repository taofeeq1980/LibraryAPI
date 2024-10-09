using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Shared.Utilities
{
    public static class CustomAuthorizationService
    {
        public static IServiceCollection AddCustomAuthorizationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(delegate (AuthenticationOptions options)
            {
                options.DefaultScheme = "Bearer";
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(delegate (JwtBearerOptions x)
            {
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    RequireSignedTokens = false,
                    SignatureValidator = (string token, TokenValidationParameters parameters) => new JwtSecurityToken(token),
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidAudience = config["IdentityServiceConfiguration:AudienceName"],
                    ValidIssuer = config["IdentityServiceConfiguration:BaseUrl"],
                    ValidateActor = false,
                    ValidateLifetime = true
                };
            });
            return services;
        }
    }
}
