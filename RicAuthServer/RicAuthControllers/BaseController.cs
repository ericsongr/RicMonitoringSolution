using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using RicAuthServer.ViewModels;

namespace RicAuthServer.RicAuthControllers
{
    public class BaseController : Controller
    {

        #region ApiResponse 

        protected BaseRestApiModel HandleApiSuccess(string message)
        {
            return new BaseRestApiModel
            {
                Payload = message,
                Errors = new List<BaseErrorModel>(),
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        protected BaseRestApiModel HandleApiSuccess(object data)
        {
            return new BaseRestApiModel
            {
                Payload = data,
                Errors = new List<BaseErrorModel>(),
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        protected BaseRestApiModel HandleApiException(object message, HttpStatusCode httpStatusCode)
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

        protected BaseRestApiModel HandleApiException(object message, string messageFields, HttpStatusCode httpStatusCode)
        {
            return new BaseRestApiModel
            {
                Payload = new List<object>(),
                Errors = new BaseErrorModel
                {
                    Message = message,
                    MessageFields = messageFields
                },
                StatusCode = (int)httpStatusCode
            };
        }

        #endregion


    }
}