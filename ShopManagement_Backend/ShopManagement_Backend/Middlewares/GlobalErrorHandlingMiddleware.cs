using ShopManagement_Backend.Responses;
using System.Text.Json;

namespace ShopManagement_Backend.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingMiddleware(
            ILogger<GlobalErrorHandlingMiddleware> logger,
            RequestDelegate next)
        {
            _logger = logger;
            _next = next;
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

                var error = new BaseResponse(
                    StatusCodes.Status500InternalServerError, "Internal Server Error");
                var response = JsonSerializer.Serialize(error);

                context.Response.ContentType = "application/json";
                context.Response.WriteAsync(response);
            }
        }
    }
}
