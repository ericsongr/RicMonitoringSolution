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
using RicAuthJwtServer.ViewModels;
using RicMonitoringAPI.Common.Model;
using BaseRestApiModel = RicMonitoringAPI.Common.Model.BaseRestApiModel;

namespace RicAuthJwtServer.Controllers
{
    [EnableCors("AllowCors")]
    [Route("api/user-profile")]
    [Authorize(Policy = "AllRoles")]
    [ApiController]
    public class UserProfileController : ApiJwtBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRegisteredDeviceService _registeredDeviceService;

        public UserProfileController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IRegisteredDeviceService registeredDeviceService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
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

            var role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult();

            user.Role = role.FirstOrDefault();

            user.RegisteredDevices = _registeredDeviceService.FindAll(id);

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
                if (result.Succeeded)
                {
                    if (await _roleManager.RoleExistsAsync(model.Role))
                        await _userManager.AddToRoleAsync(model, model.Role);
                }
                else
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
                user.IsIncomingDueDatePushNotification = model.IsIncomingDueDatePushNotification;

                user.UserName = model.UserName;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var currentRole = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();
                    if (model.Role != currentRole)
                    {
                        if (!string.IsNullOrEmpty(currentRole) && await _roleManager.RoleExistsAsync(currentRole))
                            await _userManager.RemoveFromRoleAsync(user, currentRole);

                        if (await _roleManager.RoleExistsAsync(model.Role))
                            await _userManager.AddToRoleAsync(user, model.Role);
                    }
                }

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
