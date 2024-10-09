using ApplicationServices.Customers.CommandHandler;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace LibraryAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
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
        [ProducesResponseType(typeof(Results), (int)HttpStatusCode.OK)]
        [SwaggerOperation(Summary = "Endpoint to Register Customer")]
        public async Task<IActionResult> RegistrationAsync([FromBody] RegisterCustomerCommand command)
        {
            return HandleResult(await _mediator.Send(command));
        }

        /// <summary>
        /// Endpoint to generate access token for TPP
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost("/auth/token")]
        [Consumes("application/x-www-form-urlencoded")]
        [SwaggerOperation(Summary = "Endpoint to generate access token for TPP")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GenerateAccessToken([FromForm] IFormCollection form)
        {
            var command = new GenerateAccessTokenCommand
            {
                Certificate = form["password"].ToString(),
                GrantType = form["grant_type"].ToString(),
                Scope = form["scope"].ToString(),
                ClientId = form["client_id"].ToString(),
                Code = form["code"].ToString()
            };
            return await _mediator.Send(command);
        }
    }
}
