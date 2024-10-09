using ApplicationServices.Customers.Command;
using Domain.Interfaces.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;
using Shared.Utilities;

namespace ApplicationServices.Customers.CommandHandler
{
    public class RegisterCustomerCommandHandler : IRequestHandler<RegisterCustomerCommand, Result>
    {
        private readonly ILogger<RegisterCustomerCommandHandler> _logger;
        private readonly ILibraryDbContext _context;

        public RegisterCustomerCommandHandler(ILibraryDbContext writeDbContext, ILogger<RegisterCustomerCommandHandler> logger)
        {
            _context = writeDbContext;
            _logger = logger;
        }

        public async Task<Result> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var isCustomerExist = await _context.Customers.AnyAsync(t => t.Email.ToLower() == request.Email.ToLower()
                                                                             || t.Username.ToLower() == request.Username.ToLower(),
                                                                             cancellationToken);

            if (isCustomerExist)
                return Result.Fail($"Customer : {request.Email} or {request.Username} already exist");

            await _context.Customers.AddAsync(new Domain.Entities.Customer
            {
                Email = request.Email,
                Name = request.Name,
                PasswordHash =  EncryptionUtil.GenerateSha512Hash(request.PasswordHash),
                PhoneNumber =request.PhoneNumber,
                Username = request.Username
            }, cancellationToken);

            var rowAffected = await _context.SaveChangesAsync(cancellationToken);

            return rowAffected > 0 ? Result.Ok("Successful") : Result.Fail("Customer creation failed");
        }
    }
}
