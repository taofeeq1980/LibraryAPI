using ApplicationServices.Books.Response;
using MediatR;
using Shared.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Books.Query
{
    public class GetBookQuery : IRequest<Result<GetBookDetailResponse>>
    {
        public Guid BookId { get; set; }
    }
}
