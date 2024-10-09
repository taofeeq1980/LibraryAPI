using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.BaseResponse;

namespace LibraryAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly IMediator _mediator;
        /// <param name="mediator"></param>
        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        internal IActionResult HandleResult(Result result)
        {
            if (result == null) return NotFound();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
