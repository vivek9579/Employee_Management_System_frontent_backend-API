using Employee_WebUi.Exceptions;
using Employee_WebUi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Employee_WebUi.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next
             , ILogger<GlobalExceptionMiddleware> logger)
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
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/Json";
                var response = new ErrorResponse();
                switch (ex)
                {
                    case BadRequestException:
                        response.StatusCode = 400;
                        response.Message = ex.Message;
                        break;
                    case NotFoundException:
                        response.StatusCode = 404;
                        response.Message = ex.Message;
                        break;

                    case UnauthorizedAccessException:
                        response.StatusCode = StatusCodes.Status401Unauthorized;
                        // response.Message = ex.Message;
                        response.Message = "Unauthorized access";
                        break;

                    case TimeoutException:
                        response.StatusCode = StatusCodes.Status408RequestTimeout;
                        response.Message = "Request TimeOut";
                        break;

                    case DbUpdateException:
                        response.StatusCode = StatusCodes.Status409Conflict;
                        response.Message = "Database Conflict ";
                        break;
                    default:
                        response.StatusCode = 500;
                        response.Message = "Something went wrong";
                        break;
                }
                context.Response.StatusCode = response.StatusCode;
                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response));
            }
        }
    }
}

