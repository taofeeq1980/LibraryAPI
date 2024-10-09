using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Shared.Utilities
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

    public class SignatureValidationAttribute : TypeFilterAttribute
    {

        /// <summary>
        /// 
        /// </summary>
        public SignatureValidationAttribute()
          : base(typeof(SignatureValidationAttributeImpl))
        {
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        private class SignatureValidationAttributeImpl : Attribute, IAsyncResourceFilter
        {
            private readonly ILogger _logger;
            private readonly IServiceProvider _serviceProvider;

            public SignatureValidationAttributeImpl(ILogger<SignatureValidationAttribute> logger,
                                                    IServiceProvider serviceProvider)
            {
                _logger = logger;
                _serviceProvider = serviceProvider;
            }

            public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
            {
                _logger.LogInformation($"Executing Policy Authorization...");
                _ = context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authorization);

                if (!await ValidateSignature(authorization.ToString()))
                {
                    context.Result = new UnauthorizedResult(); //new ForbidResult(JwtBearerDefaults.AuthenticationScheme);
                    return;
                }
                await next();
            }

            private async Task<bool> ValidateSignature(string accessToken)
            {
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    return false;
                }

                accessToken = accessToken.ToString().Replace("Bearer", string.Empty).Trim();
                var tokenHandler = new JwtSecurityTokenHandler();
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var configuration = scope.ServiceProvider.GetService<IConfiguration>();
                    var key = Convert.FromBase64String(configuration?["JwtSecret"] ?? string.Empty);
                    return (await tokenHandler.ValidateTokenAsync(accessToken, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                    })).IsValid;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
