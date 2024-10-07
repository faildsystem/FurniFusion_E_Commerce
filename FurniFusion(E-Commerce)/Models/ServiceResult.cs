using FurniFusion.Dtos.CartItem;
using FurniFusion.Models;

namespace FurniFusion.Models
{
    public class ServiceResult<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public static ServiceResult<T> SuccessResult(T data, string message = "Success", int statusCode = 200)
        {
            return new ServiceResult<T> { Data = data, Success = true, Message = message, StatusCode = statusCode };
        }

        public static ServiceResult<T> ErrorResult(string message, int statusCode = 500)
        {
            return new ServiceResult<T> { Success = false, Message = message, StatusCode = statusCode };
        }

        
    }

}
