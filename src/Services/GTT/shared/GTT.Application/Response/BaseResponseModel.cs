using System.Net;

namespace GTT.Application.Response
{
    public class BaseResponseModel
    {
        private const string Success = "Success";
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
        public object Dto { get; set; }

        public BaseResponseModel(object dto)
        {
            Status = HttpStatusCode.OK;
            Message = Success;
            Dto = dto;
        }

        public BaseResponseModel(HttpStatusCode status, string message)
        {
            Status = status;
            Message = message;
        }

        public BaseResponseModel(HttpStatusCode status, string message, object dto)
        {
            Status = status;
            Message = message;
            Dto = dto;
        }
    }
}
