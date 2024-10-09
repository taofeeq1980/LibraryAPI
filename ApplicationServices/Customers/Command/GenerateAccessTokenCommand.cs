using ApplicationServices.Customers.Response;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Customers.Command
{
    public class GenerateAccessTokenCommand : IRequest<Result<TokenResponse>>
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        //public string? Scope { get; set; }
        //public required string GrantType { get; set; }
        //public string? Code { get; set; }
    }
}