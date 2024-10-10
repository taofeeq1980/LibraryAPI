using ApplicationServices.Books.Query;
using ApplicationServices.Books.Response;
using Domain.Interfaces.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;
using X.PagedList;

namespace ApplicationServices.Books.QueryHandler
{
    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, Result<PagedResponse<GetBookResponse>>>
    {
        private readonly ILibraryDbContext _context;
        public GetBooksQueryHandler(ILibraryDbContext context)
        {
            _context = context;
        }
        public async Task<Result<PagedResponse<GetBookResponse>>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            var books =  await _context.Books.Select(c => new GetBookResponse
            {
                Author = c.Author,
                BookId = c.Id,
                IsAvailable = c.IsAvailable,
                ISBN = c.ISBN,
                IsReserved = c.IsReserved,
                ReturnedDate = c.ReturnedDate.ToString("yyyy-MM-dd h:mm tt"),
                Title = c.Title
            }).ToListAsync(cancellationToken);

            var pageSize = request.DisablePageLimit ? int.MaxValue : request.PageSize;
            var pagedRecord = await PagedResponse<GetBookResponse>.Create(books.OrderBy(x => x.Title).AsQueryable(), request.Page, pageSize);

            return Result.Ok(pagedRecord);
        }
    }
}
