using ApplicationServices.Books.Query;
using ApplicationServices.Books.Response;
using Domain.Interfaces.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.BaseResponse;

namespace ApplicationServices.Books.QueryHandler
{
    public class SearchBookQueryHandler : IRequestHandler<SearchBookQuery, Result<List<GetBookResponse>>>
    {
        private readonly ILibraryDbContext _context;
        public SearchBookQueryHandler(ILibraryDbContext context)
        {
            _context = context;
        }
        public async Task<Result<List<GetBookResponse>>> Handle(SearchBookQuery request, CancellationToken cancellationToken)
        {
            var searchparam = "%" + request.Title + "%";
            var books = await _context.Books.Where(b => EF.Functions.Like(b.Title, searchparam)).Select(c => new GetBookResponse
            {
                Author = c.Author,
                BookId = c.Id,
                IsAvailable = c.IsAvailable,
                ISBN = c.ISBN,
                IsReserved = c.IsReserved,
                ReturnedDate = c.ReturnedDate.ToString("yyyy-MM-dd h:mm tt"),
                Title = c.Title
            }).ToListAsync(cancellationToken);


            return Result.Ok(books);
        }
    }
}
