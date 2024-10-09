using System.Net.Mime;
using System.Net;

namespace LibraryAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // Middleware logic before passing control to the next middleware
                await context.Response.WriteAsync("Before Middleware Logic\n");

                // Call the next middleware in the pipeline
                await next(context);

                // Middleware logic after the next middleware has executed
                await context.Response.WriteAsync("After Middleware Logic\n");
            }
            catch (Exception ex)
            {
                //log the error
                _logger.LogInformation($"Something went wrong. Exception: {ex} Message: {ex.Message} StackTrace: {ex.StackTrace}");
                //handle the exception
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogInformation($"Middleware Error:::{exception.Message} {exception.StackTrace}");
            //context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = exception is BadHttpRequestException ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync("Request cannot be processed. Try again later");
        }
    }
}