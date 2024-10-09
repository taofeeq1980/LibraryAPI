using ApplicationServices.Books.Command;
using Domain.Entities;
using Domain.Interfaces.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;

namespace ApplicationServices.Books.CommandHandler
{
    internal class BorrowBookCommandHandler : IRequestHandler<BorrowBookCommand, Result>
    {
        private readonly ILibraryDbContext _context;
        private readonly ILogger<BorrowBookCommandHandler> _logger;
        public BorrowBookCommandHandler(ILibraryDbContext context, ILogger<BorrowBookCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Result> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
        {
            //var bookIds = request.LoanBooks.Select(b => b.BookId).ToList();
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == request.BookId, cancellationToken);
            if (book == null) return Result.Fail("No book record found");

            if (!book.IsAvailable)
                return Result.Fail($"{book.Title} already borrowed another customer");

            if (book.IsReserved)
            {
                var reservedBook = await _context.Reservations.FirstOrDefaultAsync(b => b.BookId == request.BookId
                                                                                        && !b.IsReservationExpired, cancellationToken);
                if (reservedBook != null && reservedBook.CustomerId != request.BookId)
                    return Result.Fail($"{book.Title} reserved by another customer");
            }
            book.IsReserved = false;
            book.IsAvailable = false;
            book.ReturnedDate = DateTime.UtcNow.AddHours(request.Tenor);
            _context.Books.Add(book);

            await _context.Loans.AddAsync(new Loan
            {
                BookId = request.BookId,
                CustomerId = Guid.NewGuid()
            }, cancellationToken);
            _context.Books.Update(book);


            var rowAffected = await _context.SaveChangesAsync(cancellationToken);

            return rowAffected > 0 ? Result.Ok($"{book.Title} borrowed successfully") : Result.Fail($"{book.Title} borowing failed");

        }
    }
}