using MediatR;
using Shared.BaseResponse;
using System.ComponentModel.DataAnnotations;

namespace ApplicationServices.Customers.Command
{
    public class RegisterCustomerCommand : IRequest<Result>
    {
        public required string Name { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }
        public required string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 8 and 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Password must be alphanumeric (letters and numbers only).")]
        public required string PasswordHash { get; set; }
        public required string PhoneNumber { get; set; }
    }
}