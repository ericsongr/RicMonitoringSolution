using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using RicAuthJwtServer.Models;
using RicMonitoringAPI.Common.Model;

namespace RicAuthJwtServer.Controllers
{
    public class ApiJwtBaseController : ControllerBase
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
