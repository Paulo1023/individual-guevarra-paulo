using System.Net;
using System.Text.Json;

namespace Nexus.API.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next; //determine if will go to the next middleware or stop process
        private readonly ILogger<GlobalExceptionHandler> _logger; // logs the messages

        // constructor to set the _next and _logger
        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        // if no exception the process continue, else produce the custom error
        public async Task InvokeAsync(HttpContext context) 
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // for dev error 
                _logger.LogError(ex, "An unhandled exception has occured");
                // for user error
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // set status code to internal server error 500 then custom message for the user
            var response = new 
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error from the custom handler. Please try again later."
            };
            // return the error status and message in json format
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }


    }
}
