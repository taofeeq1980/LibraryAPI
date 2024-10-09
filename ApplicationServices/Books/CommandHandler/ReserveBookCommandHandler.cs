using ApplicationServices.Books.Command;
using Domain.Entities;
using Domain.Interfaces.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;

namespace ApplicationServices.Books.CommandHandler
{
    public class ReserveBookCommandHandler : IRequestHandler<ReserveBookCommand, Result>
    {
        private readonly ILibraryDbContext _context;
        private readonly ILogger<ReserveBookCommandHandler> _logger;
        public ReserveBookCommandHandler(ILibraryDbContext context, ILogger<ReserveBookCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Result> Handle(ReserveBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == request.BookId, cancellationToken);
            if (book == null) return Result.Fail("No book record found");

            if (!book.IsAvailable || book.IsReserved)
                return Result.Fail($"{book.Title} not available for reservation");

            book.IsReserved = true;
            //book.ReturnedDate = DateTime.UtcNow.AddHours(24);

            await _context.Reservations.AddAsync(new Reservation
            {
                BookId = request.BookId,
                CustomerId = Guid.NewGuid()
            }, cancellationToken);

            _context.Books.Update(book);
            var rowAffected = await _context.SaveChangesAsync(cancellationToken);

            return rowAffected > 0 ? Result.Ok($"{book.Title} reserved successfully") : Result.Fail($"{book.Title} reservation failed");
        }
    }
}
