using Microsoft.AspNetCore.Http;

namespace ShopManagement_Backend_Application.Models
{
    public class BaseResponse
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public BaseResponse(string message)
        {
            Status = StatusCodes.Status200OK;
            Message = message;
        }

        public BaseResponse(object data)
        {
            Status = StatusCodes.Status200OK;
            Message = "Success";
            Data = data;
        }

        // Failed to execute
        public BaseResponse(int stautus, string message)
        {
            Status = stautus;
            Message = message;
        }
    }
}
