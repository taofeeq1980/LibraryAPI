using ApplicationServices.Books.Response;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Books.Query
{
    public class SearchBookQuery : IRequest<Result<List<GetBookResponse>>>
    {
        public required string Title { get; set; }
    }
}