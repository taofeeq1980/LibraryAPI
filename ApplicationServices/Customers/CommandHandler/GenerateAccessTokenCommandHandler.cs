using ApplicationServices.Customers.Command;
using ApplicationServices.Customers.Response;
using Domain.Interfaces.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.BaseResponse;
using Shared.Exceptions;
using Shared.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApplicationServices.Customers.CommandHandler
{
    public class GenerateAccessTokenCommandHandler : IRequestHandler<GenerateAccessTokenCommand, Result<TokenResponse>>
    {
        private readonly ILibraryDbContext _context;
        private readonly IConfiguration _config;
        public string Secret { get; }

        public GenerateAccessTokenCommandHandler(IConfiguration config, ILibraryDbContext context)
        {
            _context = context;
            _config = config;
            Secret = _config["JwtSecret"] ?? string.Empty;
        }

        public async Task<Result<TokenResponse>> Handle(GenerateAccessTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                throw new BadRequestException("username or password is missing or invalid.");
            }
            var customer = await _context.Customers.FirstOrDefaultAsync(t => t.Username.ToLower() == request.Username.ToLower(), cancellationToken);

            if (customer == null)
            {
                throw new BadRequestException("username or password is invalid.");
            }

            if (customer.PasswordHash != EncryptionUtil.GenerateSha512Hash(request.Password))
                throw new BadRequestException("username or password is invalid.");

            byte[] key = Convert.FromBase64String(Secret);
            SymmetricSecurityKey securityKey = new(key);
            DateTime expiration = DateTime.UtcNow.AddMinutes(5);
            long seconds = (long)expiration.Subtract(new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day)).TotalSeconds;
            SecurityTokenDescriptor descriptor = new()
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Name, request.Username),
                    new Claim(ClaimTypes.Email, customer.Email),
                    new Claim("customerId", customer.Id.ToString())
                ]),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            string jwtToken = handler.WriteToken(token);

            // Create a JSON object containing the token and additional details
            var tokenResponse = new TokenResponse
            {
                AccessToken = jwtToken,
                ExpiresIn = seconds
            };
            return Result.Ok(tokenResponse);
        }
    }
}
