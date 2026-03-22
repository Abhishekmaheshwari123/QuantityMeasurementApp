using System.Text.Json;
using QuantityMeasurement.Api.Contracts;
using QuantityMeasurement.Domain.Exceptions;

namespace QuantityMeasurement.Api.Middleware
{
    public sealed class GlobalExceptionMiddleware
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
            catch (QuantityMeasurementException ex)
            {
                _logger.LogWarning(ex, "Domain validation error while processing request.");
                await WriteErrorResponseAsync(context, StatusCodes.Status400BadRequest, "Quantity Measurement Error", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception while processing request.");
                await WriteErrorResponseAsync(context, StatusCodes.Status500InternalServerError, "Internal Server Error", ex.Message);
            }
        }

        private static async Task WriteErrorResponseAsync(HttpContext context, int statusCode, string error, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var body = new ApiErrorResponse
            {
                Status = statusCode,
                Error = error,
                Message = message,
                Path = context.Request.Path
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(body, JsonOptions));
        }
    }
}
