using System;
using System.Collections.Generic;
using System.Net;
using RicMonitoringAPI.Common.Model;
using RicMonitoringAPI.RoomRent.Controllers;

namespace RicMonitoringAPI.Infrastructure.Helpers
{
    public static class HandleApi
    {
        public  static BaseRestApiModel Exception(string message, HttpStatusCode httpStatusCode)
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
