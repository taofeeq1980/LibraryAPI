using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Customers.CommandHandler
{
    public class RegisterCustomerCommand : IRequest<Result>
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string PhoneNumber { get; set; }
    }
}

