using ShopManagement_Backend.Responses;
using System;
using System.Net;
using System.Text.Json;

namespace ShopManagement_Backend.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(
            ILogger<ExceptionHandlingMiddleware> logger,
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unexpected error occurred.");

            BaseResponse response;

            switch(context.Response.StatusCode)
            {
                case (int)HttpStatusCode.BadRequest:
                    response = new BaseResponse(
                        StatusCodes.Status400BadRequest, "Bad request");
                    break;
                case (int)HttpStatusCode.NotFound:
                    response = new BaseResponse(
                        StatusCodes.Status404NotFound, "The request key not found");
                    break;
                case (int)HttpStatusCode.Unauthorized:
                    response = new BaseResponse(
                        StatusCodes.Status401Unauthorized, "Unauthorized");
                    break;
                default:
                    response = new BaseResponse(
                        StatusCodes.Status500InternalServerError, "Internal Server Error");
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.Status;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
