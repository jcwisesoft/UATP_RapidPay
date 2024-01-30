using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Code_Test_UATP_RapidPay.Controllers
{
    public abstract class BaseAPIController : Controller
    {
        [NonAction]
        public override OkObjectResult Ok(object value)
        {
            return base.Ok(new Result
            {
                Status = HttpStatusCode.OK,
                Message = "",
                Data = value
            });
        }

        [NonAction]
        public OkObjectResult Ok(object value, string message = "")
        {
            return base.Ok(new Result
            {
                Status = HttpStatusCode.OK,
                Message = message,
                Data = value
            });
        }

        [NonAction]
        public BadRequestObjectResult BadRequest(object value, string message = "")
        {
            return base.BadRequest(new Result
            {
                Status = HttpStatusCode.BadRequest,
                Message = message,
                Data = value
            });
        }

    }

    internal class Result
    {
        public string Message { get; set; }
        public object Data { get; set; }
        public HttpStatusCode Status { get; set; }
    }

}
