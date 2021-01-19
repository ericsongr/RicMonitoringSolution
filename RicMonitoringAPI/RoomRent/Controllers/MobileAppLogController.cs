using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using RicEntityFramework.RoomRent.Interfaces;
using RicModel.RoomRent;
using RicModel.RoomRent.Dtos;
using RicMonitoringAPI.Common.Model;

namespace RicMonitoringAPI.RoomRent.Controllers
{
    [AllowAnonymous]
    [Route("mobile-app-log")]
    [ApiController]
    public class MobileAppLogController : Controller
    {
        private readonly IMobileAppLogRepository _mobileAppLogRepository;

        public MobileAppLogController(IMobileAppLogRepository mobileAppLogRepository)
        {
            _mobileAppLogRepository = mobileAppLogRepository ?? throw new ArgumentNullException(nameof(IMobileAppLogRepository));
        }
        [HttpPost()]
        public IActionResult Create([FromBody] MobileAppLogDto model)
        {
            _mobileAppLogRepository.Add(new MobileAppLog
            {
                Type = model.Type, 
                LogInfo = model.LogInfo,
                UtcCreatedDateTime = DateTime.UtcNow
            });

            _mobileAppLogRepository.Commit();

            return Ok(new BaseRestApiModel
            {
                Payload = "Mobile app log has been added.",
                Errors = new List<BaseErrorModel>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }
    }
}
