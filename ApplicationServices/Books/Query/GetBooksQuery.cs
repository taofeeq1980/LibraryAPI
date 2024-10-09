using ApplicationServices.Books.Response;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Books.Query
{
    public class GetBooksQuery : PaginationParameter, IRequest<Result<PagedResponse<GetBookResponse>>>
    {
    }
}