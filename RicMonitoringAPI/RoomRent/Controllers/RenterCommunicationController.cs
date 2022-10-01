using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RicCommunication.Interface;
using RicEntityFramework.Interfaces;
using RicModel.Enumeration;
using RicMonitoringAPI.Common.Model;
using RicMonitoringAPI.RoomRent.ViewModels.ApiModels;

namespace RicMonitoringAPI.RoomRent.Controllers
{
    [AllowAnonymous]
    [Route("api/renter-communication")]
    [ApiController]
    public class RenterCommunicationController : ApiBaseController
    {
        private readonly ICommunicationService _communicationService;
        private readonly IPushNotificationGateway _pushNotificationGateway;

        public RenterCommunicationController(
            ICommunicationService communicationService,
            IPushNotificationGateway pushNotificationGateway)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _pushNotificationGateway = pushNotificationGateway ?? throw new ArgumentNullException(nameof(pushNotificationGateway));
        }

        [Route("user/push-notifications/{userId}")]
        public IActionResult GetPushNotifications(string userId)
        {
            try
            {
                var comms = _communicationService.GetRenter(58, CommunicationType.PushNotifications);

                if (comms.Any())
                {
                   return Ok(new BaseRestApiModel
                    {
                        Payload = new
                        {
                            notifications = comms.OrderByDescending(o => o.CommunicationUtcdateTime).Take(20).Select(p => new
                            {
                                SentOn = p.CommunicationUtcdateTime ?? DateTime.UtcNow,
                                Message = p.CommunicationText,
                                NotificationId = p.Id
                            })
                        },
                        Errors = new List<BaseErrorModel>(),
                        StatusCode = (int)HttpStatusCode.OK
                    });
                }

                return Ok(HandleApiException("No data found.", HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                return Ok(HandleApiException(ex.ToString(), HttpStatusCode.InternalServerError));
            }

        }

        [Route("user/push-notifications")]   
        public IActionResult GetPushNotifications(PushNotificationApiModel model)
        {
            try
            {
                var comms = _communicationService.GetRenter(model.RenterId, CommunicationType.PushNotifications);
                
                if (comms.Any())
                {
                    int totalCount = comms.Count;
                    int totalUnread = comms.Count(q => q.HasRead == false);
                    int pageNo = model.PageNumber == 0 ? 1 : model.PageNumber;
                    pageNo--;
                    var commsPaged = comms.OrderBy(q => q.HasRead).Skip(model.PageSize * pageNo).Take(model.PageSize);


                    return Ok(new BaseRestApiModel
                    {
                        Payload = new
                        {
                            notifications = commsPaged.Select(p => new
                            {
                                SentOn = p.CommunicationUtcdateTime ?? DateTime.UtcNow,
                                Message = p.CommunicationText,
                                NotificationId = p.Id
                            }),
                            totalCount,
                            totalUnread
                        },
                        Errors = new List<BaseErrorModel>(),
                        StatusCode = (int)HttpStatusCode.OK
                    });
                }

                return Ok(HandleApiException("No data found.", HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                return Ok(HandleApiException(ex.ToString(), HttpStatusCode.InternalServerError));
            }

        }

        [Route("user/push-notification-read/{notificationId:int}")]
        public IActionResult PutPushNotification(int notificationId)
        {
            try
            {
                var comm = _communicationService.GetById(notificationId);
                if (comm != null)
                {

                    comm.HasRead = true;
                    _communicationService.Save(comm);
                    
                    return Ok(new BaseRestApiModel
                    {
                        Payload = "success",
                        Errors = new List<BaseErrorModel>(),
                        StatusCode = (int)HttpStatusCode.OK
                    });
                }
                return Ok(HandleApiException("No data found.", HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                return Ok(HandleApiException(ex.ToString(), HttpStatusCode.InternalServerError));
            }
        }

        [Route("user/sample")]
        public IActionResult SendSample(string message)
        {
            var portalUserId = "05b6e3bd-c9aa-4ce8-9912-3ccaa0abf892";
            var devicesIds = new List<string> { "33fa525a-93ed-4201-9bd6-f28db0eb448e" };
            _pushNotificationGateway.SendNotification(portalUserId, devicesIds, "first title", message);
            return Ok("sent");
        }

        [Route("user/update")]
        public IActionResult Update()
        {
            var portalUserId = "05b6e3bd-c9aa-4ce8-9912-3ccaa0abf892"; //Guid.NewGuid().ToString();
         
            var devicesId = "edbe35fd-9a23-4507-a59f-e3d898680998";
            
            var success = _pushNotificationGateway.UpdateDeviceExternalUserId(portalUserId, devicesId);

            return Ok(success ? "Sent" : "Failed");
        }
    }
}
