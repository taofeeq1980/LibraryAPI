using ApplicationServices.Customers.Command;
using ApplicationServices.Customers.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.BaseResponse;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace LibraryAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public CustomerController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Register Customer Endpoint
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("registration")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [SwaggerOperation(Summary = "Endpoint to Register Customer")]
        public async Task<IActionResult> RegistrationAsync([FromBody] RegisterCustomerCommand command)
        {
            return HandleResult(await _mediator.Send(command));
        }

        /// <summary>
        /// Endpoint to generate access token for Customer
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("/auth/token")]
        [SwaggerOperation(Summary = "Endpoint to generate access token for Customer")]
        [ProducesResponseType(typeof(Result<TokenResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<TokenResponse>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GenerateAccessToken([FromBody] GenerateAccessTokenCommand command)
        {
            return HandleResult(await _mediator.Send(command));
        }
    }
}