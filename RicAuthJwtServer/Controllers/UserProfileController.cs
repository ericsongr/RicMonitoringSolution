using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RicAuthJwtServer.Application.Interfaces;
using RicAuthJwtServer.Data;
using RicAuthJwtServer.Data.Extensions;
using RicMonitoringAPI.Common.Model;

namespace RicAuthJwtServer.Controllers
{
    [EnableCors("AllowCors")]
    [Route("api/user-profile")]
    [Authorize(Policy = "AllRoles")]
    [ApiController]
    public class UserProfileController : ApiJwtBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRegisteredDeviceService _registeredDeviceService;

        public UserProfileController(
            UserManager<ApplicationUser> userManager,
            IRegisteredDeviceService registeredDeviceService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager)); 
            _registeredDeviceService = registeredDeviceService ?? throw new ArgumentNullException(nameof(registeredDeviceService));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await Task.Run(() => 
                _userManager.Users.Where(o => o.UserName != "RunDailyBatch").Select(o => o.Projection()).ToList());

            return Ok(new BaseRestApiModel
            {
                Payload = users
            });
        }

        [HttpGet]
        [Route("profile")]
        public async Task<IActionResult> Profile(string id)
        {

            var user = await _userManager.FindByIdAsync(id);

            return Ok(new BaseRestApiModel
            {
                Payload = user
            });
        }

        [HttpPost]
        [Route("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] ApplicationUser model)
        {
            var user = await _userManager.FindByEmailAsync(model.UserName);
            if (user != null)
            {
                return Ok(HandleApiException("Username already exists", HttpStatusCode.NotFound));
            }
            else
            {
                model.Id = Guid.NewGuid().ToString(); // for new user
                var result = await _userManager.CreateAsync(model, "Pa$$w0rd");
                if (!result.Succeeded)
                {
                    return Ok(HandleApiException("User creation failed.", HttpStatusCode.NotFound));
                }

                return Ok(new BaseRestApiModel
                {
                    Payload = "success",
                    Errors = new List<BaseError>(),
                    StatusCode = (int)HttpStatusCode.OK
                });
            }

        }

        [HttpPut]
        [Route("update-profile")]
        public async Task<IActionResult> UpdateUser([FromBody] ApplicationUser model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.MobileNumber = model.MobileNumber;
                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;

                user.IsReceiveDueDateAlertPushNotification = model.IsReceiveDueDateAlertPushNotification;
                user.IsPaidPushNotification = model.IsPaidPushNotification;

                user.UserName = model.UserName;

                await _userManager.UpdateAsync(user);

                return Ok(new BaseRestApiModel
                {
                    Payload = "success",
                    Errors = new List<BaseError>(),
                    StatusCode = (int)HttpStatusCode.OK
                });
            }
            else
            {
                return Ok(HandleApiException("User does not exists", HttpStatusCode.NotFound));
            }
            
        }
    }
}
