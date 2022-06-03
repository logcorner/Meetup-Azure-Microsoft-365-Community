using System.Net;
using System.Text.Json;
using TodoList.Application.Exceptions;

namespace TodoList.WebApi.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;

        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var logger = _loggerFactory.CreateLogger("ExceptionMiddleware");
                logger.LogError($"Something went wrong: {ex.StackTrace}");
                await HandleExceptionAsync(ex, httpContext);
            }
        }

        private static Task HandleExceptionAsync(Exception ex, HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorCode = (int)HttpStatusCode.InternalServerError;
            var message = "Internal Server Error.";
            if (ex is Domain.Exceptions.BaseException domainException)
            {
                errorCode = domainException.ErrorCode;
                message = domainException.Message;
            }
            if (ex is BaseException applicationException)
            {
                errorCode = applicationException.ErrorCode;
                message = applicationException.Message;
            }

            context.Response.StatusCode = errorCode;
            return context.Response.WriteAsync(JsonSerializer.Serialize(
                new
                {
                    errorCode,
                    message
                }));
        }
    }
}