using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Books.Command
{
    public class ReserveBookCommand : IRequest<Result>
    {
        public Guid BookId { get; set; }     
    }
}