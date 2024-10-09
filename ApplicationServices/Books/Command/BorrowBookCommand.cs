using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Books.Command
{
    public class BorrowBookCommand: IRequest<Result>
    {
        //public required List<LoanBookRequest> LoanBooks {  get; set; }
        public Guid? BookId { get; set; }
        public int Tenor { get; set; }
    }

    //public class LoanBookRequest
    //{
    //    public Guid? BookId { get; set; }
    //    public int Tenor { get; set; }
    //}
}
