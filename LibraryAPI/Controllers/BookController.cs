using ApplicationServices.Books.Command;
using ApplicationServices.Books.Query;
using ApplicationServices.Books.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.BaseResponse;
using Shared.Utilities;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace LibraryAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [SignatureValidation]
    public class BookController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public BookController(IMediator mediator) : base(mediator)
        {
        }
        /// <summary>
        /// Endpoint to get list of books
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("get-books")]
        [ProducesResponseType(typeof(PagedResponse<GetBookResponse>), (int)HttpStatusCode.OK)]
        [SwaggerOperation(Summary = "Endpoint to get list of books")]
        public async Task<IActionResult> GetAllBooks([FromQuery] GetBooksQuery query)
        {
            return HandleResult(await _mediator.Send(query));
        }
        /// <summary>
        /// Endpoint to get a book details
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpGet("get-book/{bookId}")]
        [ProducesResponseType(typeof(PagedResponse<GetBookResponse>), (int)HttpStatusCode.OK)]
        [SwaggerOperation(Summary = "Endpoint to get list of books")]
        public async Task<IActionResult> GetBook(Guid bookId)
        {
            return HandleResult(await _mediator.Send(new GetBookQuery { BookId = bookId }));
        }
        /// <summary>
        /// Endpoint to reserve a book
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpPost("reserve/{bookId}")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [SwaggerOperation(Summary = "Endpoint to reserve a book")]
        public async Task<IActionResult> ReserveBook(Guid bookId)
        {
            return HandleResult(await _mediator.Send(new ReserveBookCommand { BookId = bookId }));
        }
        /// <summary>
        /// Endpoint to borrow a book
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("borrow")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [SwaggerOperation(Summary = "Endpoint to borrow a book")]
        public async Task<IActionResult> BorrowBook([FromBody] BorrowBookCommand command)
        {
            return HandleResult(await _mediator.Send(command));
        }
        /// <summary>
        /// Endpoint to set notification for customer when a book is avaialble
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("notify")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [SwaggerOperation(Summary = "Endpoint to set notification for customer when a book is avaialble")]
        public async Task<IActionResult> NotifyWhenAvailable([FromBody] NotifyAboutBookCommand command)
        {
            return HandleResult(await _mediator.Send(command));
        }
    }
}