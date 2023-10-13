using Microsoft.AspNetCore.Http;

namespace LettersRegistration.WebAPI.Domain
{
    public class BaseResponse<T> : IBaseResponse<T>
    {
        public string Message { get; set; }

        public int StatusCode { get; set; }

        

    }

    public interface IBaseResponse<T>
    {
        string Message { get; }
        int StatusCode { get; }
        
    }
}
