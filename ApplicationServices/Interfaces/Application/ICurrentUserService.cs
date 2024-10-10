using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace ApplicationServices.Interfaces.Application
{
    public interface ICurrentUserService
    {
        string CurrentUsername();
        Guid CurrentUserId();
    }

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string CurrentUsername()
        {
            var unique_name = JwtSecurityToken().Claims.First(claim => claim.Type == "unique_name").Value;
            return unique_name;

        }
        public Guid CurrentUserId()
        {
            var customerId = JwtSecurityToken()?.Claims?.First(claim => claim.Type == "customerId")?.Value;
            return Guid.Parse(customerId ?? Guid.NewGuid().ToString());

        }
        private JwtSecurityToken JwtSecurityToken()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"];
            string? text = token?.ToString();
            int length = "Bearer".Length;
            if (string.IsNullOrWhiteSpace(text)) return new JwtSecurityToken();
            var serializedToken = text[length..].ToString().Trim();
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(serializedToken);
        }
    }
}
