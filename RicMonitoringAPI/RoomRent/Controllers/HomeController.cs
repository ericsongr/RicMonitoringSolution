using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using RicMonitoringAPI.Common.Model;

namespace RicMonitoringAPI.RoomRent.Controllers
{
    public class ApiBaseController : ControllerBase
    {
        public BaseRestApiModel HandleApiException(string message, HttpStatusCode httpStatusCode)
        {
            return new BaseRestApiModel
            {
                Payload = new List<object>(),
                Errors = new BaseErrorModel
                {
                    Message = message,
                    MessageFields = ""
                },
                StatusCode = (int)httpStatusCode
            };
        }
    }
}
