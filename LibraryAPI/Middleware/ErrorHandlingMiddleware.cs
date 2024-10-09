using System.Net.Mime;
using System.Net;
using Newtonsoft.Json;
using Shared.BaseResponse;
using Shared.Exceptions;

namespace LibraryAPI.Middleware
{
    /// <summary>
    /// Middleware for handling all application errors. Also logs the error too.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        /// <summary>
        /// Constructor for <see cref="ErrorHandlingMiddleware"/>
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, IWebHostEnvironment env)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                //log the error
                _logger.LogInformation($"Something went wrong. Exception: {ex} Message: {ex.Message} StackTrace: {ex.StackTrace}");
                //handle the exception
                await HandleExceptionAsync(context, ex, env);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, IWebHostEnvironment env)
        {
            var stackTrace = string.Empty;

            var response = context.Response;
            switch (exception)
            {
                case var _ when exception is BadRequestException badRequestException:
                    // bad request error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case var _ when exception is NotFoundException notFoundException:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case var _ when exception is UnAuthorizedException unAuthorizedException:
                    // unAuthorizedException error
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case var _ when exception is ArgumentIsNullException argumentIsNullException:
                    // argumentIsNullException error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case var _ when exception is NegativeOrZeroException negativeOrZeroException:
                    // argumentIsNullException error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case var _ when exception is EntityNotFoundException entityNotFoundxException:
                    // entityNotFoundException error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case var _ when exception is InvalidEntityException invalidEntityException:
                    // InvalidEntityException error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    if (env.IsEnvironment("Development"))
                    {
                        stackTrace = exception.StackTrace;
                    }
                    break;
            }

            var errorlogged = Result.Fail(exception.InnerException,
                string.IsNullOrEmpty(exception.Message) ? "Error occured" : exception.Message + $" ::: {exception.StackTrace} ::: {exception.InnerException}",
                response.StatusCode.ToString());

            var errMessage = response.StatusCode == StatusCodes.Status400BadRequest ? exception.Message : "Error occured, please try again later";
            var error = Result.Fail(errMessage, response.StatusCode.ToString());

            var result = JsonConvert.SerializeObject(error); //, JsonSerializerUtility.CamelCaseSerializerSettings());
            _logger.LogInformation($"Middleware Error handling:::{errorlogged}");
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = response.StatusCode;
            return context.Response.WriteAsync(result);
        }
    }
}