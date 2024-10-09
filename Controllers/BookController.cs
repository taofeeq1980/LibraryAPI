using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        //[HttpGet]
        //public async Task<IActionResult> GetAllBooks()
        //{
        //    var books = await _bookService.GetAllBooksAsync();
        //    return Ok(books);
        //}

        //[HttpPost("reserve")]
        //[Authorize]
        //public async Task<IActionResult> ReserveBook(int bookId)
        //{
        //    var customerId = int.Parse(User.FindFirst("id").Value);  // Assuming the claim is present
        //    var result = await _bookService.ReserveBookAsync(bookId, customerId);
        //    if (result) return Ok("Book reserved successfully.");
        //    return BadRequest("Book is not available for reservation.");
        //}

        //[HttpPost("notify")]
        //[Authorize]
        //public async Task<IActionResult> NotifyWhenAvailable(int bookId)
        //{
        //    var customerId = int.Parse(User.FindFirst("id").Value);
        //    var result = await _bookService.NotifyWhenAvailableAsync(bookId, customerId);
        //    if (result) return Ok("You will be notified when the book is available.");
        //    return BadRequest("Unable to set notification.");
        //}
    }
}
