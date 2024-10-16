﻿using ApplicationServices.Books.Response;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Books.Query
{
    public class GetBookQuery : IRequest<Result<GetBookDetailResponse>>
    {
        public Guid BookId { get; set; }
    }
}
