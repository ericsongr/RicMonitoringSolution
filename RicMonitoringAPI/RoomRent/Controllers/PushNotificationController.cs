using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RicCommunication.Interface;
using RicMonitoringAPI.Common.Constants;
using RicMonitoringAPI.Common.Model;
using RicMonitoringAPI.RoomRent.ViewModels.ApiModels;

namespace RicMonitoringAPI.RoomRent.Controllers
{
    [Authorize(Policy = "ProcessTenantsTransaction")]
    [Route("api/push-notification")]
    [ApiController]
    public class PushNotificationController : Controller
    {
        private readonly IPushNotificationGateway _pushNotificationGateway;

        public PushNotificationController(
            IPushNotificationGateway pushNotificationGateway)
        {
            _pushNotificationGateway = pushNotificationGateway ?? throw new ArgumentNullException(nameof(pushNotificationGateway));
        }
        
        [HttpPost()]
        public async Task<IActionResult> Test([FromBody] UserPushNotificationApiModel userPushNotification)
        {
            
            await Task.Run(() =>
            {
                string title = "";
                string message = "";

                switch (userPushNotification.Source)
                {
                    case SourcePushNotificationConstant.IncomingDueDatePushNotification:
                        title = "Incoming Due Date Alert";
                        message = "Test push notification message of Incoming Due Date for renter";
                        break;
                    case SourcePushNotificationConstant.ReceiveDueDateAlertPushNotification:
                        title = "Due Date Alert";
                        message = "Test push notification message of Due Date for renter";
                        break;
                    case SourcePushNotificationConstant.PaidPushNotification:
                        title = "Settled Payment Alert";
                        message = "Test push notification message of Settled Payment Due Date for renter";
                        break;
                     case SourcePushNotificationConstant.BatchProcessCompletedPushNotification:
                        title = "Batch Process Completed Alert";
                        message = "Test push notification message for Completed Batch Process";
                        break;
                        
                }
                var deviceIds = new List<string>() { userPushNotification.DeviceId };

                _pushNotificationGateway.SendNotification(userPushNotification.PortalUserId, deviceIds, title , message);


            });
            
            return Ok(new BaseRestApiModel
            {
                Payload = "success",
                Errors = new List<BaseErrorModel>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }
    }
}
