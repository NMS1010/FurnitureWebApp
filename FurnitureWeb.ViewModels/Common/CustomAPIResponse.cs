using System.Collections.Generic;

namespace FurnitureWeb.ViewModels.Common
{
    public class CustomAPIResponse<T>
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; }
        public bool IsSuccesss { get; set; }

        public static CustomAPIResponse<T> Success(T data, int statusCode)
        {
            return new CustomAPIResponse<T>() { Data = data, StatusCode = statusCode, IsSuccesss = true };
        }

        public static CustomAPIResponse<T> Success(int statusCode)
        {
            return new CustomAPIResponse<T>() { StatusCode = statusCode, IsSuccesss = true };
        }

        public static CustomAPIResponse<T> Fail(int statusCode, string error)
        {
            return new CustomAPIResponse<T>() { StatusCode = statusCode, Errors = new List<string>() { error }, IsSuccesss = false };
        }

        public static CustomAPIResponse<T> Fail(int statusCode, List<string> errors)
        {
            return new CustomAPIResponse<T>() { StatusCode = statusCode, Errors = errors, IsSuccesss = false };
        }
    }
}