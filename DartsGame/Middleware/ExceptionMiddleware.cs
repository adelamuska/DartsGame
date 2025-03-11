using System;
using System.Net;
using System.Text.Json;

namespace DartsGame.Middleware
{
    public class ExceptionMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            var response = context.Response;
            response.ContentType = "application/json";


            response.StatusCode = exception switch
            {

                ArgumentException => (int)HttpStatusCode.BadRequest,
                InvalidOperationException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var errorResponse = new
            {
                response.StatusCode,
                exception.Message
            };

            return response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }

       
    }
}
