using ApplicationServices.Books.Query;
using ApplicationServices.Books.Response;
using Domain.Entities;
using Domain.Interfaces.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Books.QueryHandler
{
    public class GetBookQueryHandler : IRequestHandler<GetBookQuery, Result<GetBookDetailResponse>>
    {
        private readonly ILibraryDbContext _context;
        private readonly ILogger<GetBookQueryHandler> _logger;
        public GetBookQueryHandler(ILibraryDbContext context, ILogger<GetBookQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Result<GetBookDetailResponse>> Handle(GetBookQuery request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.Where(b => b.Id == request.BookId).Select(c => new GetBookDetailResponse
            {
                Author = c.Author,
                BookId = c.Id,
                IsAvailable = c.IsAvailable,
                ISBN = c.ISBN,
                IsReserved = c.IsReserved,
                ReturnedDate = c.ReturnedDate.ToString("yyyy-MM-dd h:mm tt"),
                Title = c.Title
            }).FirstOrDefaultAsync(cancellationToken) ?? new GetBookDetailResponse();

            if (book == null || string.IsNullOrWhiteSpace(book?.Title)) return Result.Fail<GetBookDetailResponse>("No Book record found");

            var loanBook = !book.IsAvailable ? await _context.Loans.Where(c => c.BookId == request.BookId)
                                                             .Include(c => c.Customer).ToListAsync(cancellationToken) : [];
            book.BorrowedBy = loanBook.Select(c => new BorrowedBy
            {
                CustomerName = c?.Customer?.Name,
                Email = c?.Customer?.Email,
                PhoneNumber = c?.Customer?.PhoneNumber
            }).FirstOrDefault();

            var reserveBook = book.IsReserved ? await _context.Reservations.Where(c => c.BookId == request.BookId)
                                                             .Include(c => c.Customer).ToListAsync(cancellationToken) : [];
            book.ReservedBy = reserveBook.Select(c => new BorrowedBy
            {
                CustomerName = c?.Customer?.Name,
                Email = c?.Customer?.Email,
                PhoneNumber = c?.Customer?.PhoneNumber
            }).FirstOrDefault();
            book.NoOfDays = loanBook?.FirstOrDefault()?.Tenor ?? 0;
            book.DateBorrowed = loanBook?.FirstOrDefault()?.DateAdded?.ToString("yyyy-MM-dd h:mm tt");
            return Result.Ok(book);
        }
    }
}
