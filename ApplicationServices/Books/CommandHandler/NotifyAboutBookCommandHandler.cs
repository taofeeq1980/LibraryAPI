using ApplicationServices.Books.Command;
using ApplicationServices.Interfaces.Application;
using Domain.Entities;
using Domain.Interfaces.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;

namespace ApplicationServices.Books.CommandHandler
{
    public class NotifyAboutBookCommandHandler : IRequestHandler<NotifyAboutBookCommand, Result>
    {
        private readonly ILibraryDbContext _context;
        private readonly ILogger<NotifyAboutBookCommandHandler> _logger;
        private readonly ICurrentUserService _currentUserService;
        public NotifyAboutBookCommandHandler(ILibraryDbContext context, ILogger<NotifyAboutBookCommandHandler> logger, ICurrentUserService currentUserService)
        {
            _context = context;
            _logger = logger;
            _currentUserService = currentUserService;
        }
        public async Task<Result> Handle(NotifyAboutBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == request.BookId, cancellationToken);
            if (book == null) return Result.Fail("No book record found");

            await _context.Notifications.AddAsync(new Notification
            {
                CustomerId = _currentUserService.CurrentUserId(),
                NotifyBy = request.NotificationChannel,
                Notify = false
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            var rowAffected = await _context.SaveChangesAsync(cancellationToken);

            return rowAffected > 0 ? Result.Ok($"Notification on {book.Title} set successfully") : Result.Fail($"Filed to set notification on {book.Title}");
        }
    }
}

