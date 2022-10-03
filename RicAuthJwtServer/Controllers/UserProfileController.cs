using System;
using System.Linq;
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
    [ApiController]
    public class UserProfileController : ControllerBase
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

        [Authorize(Policy = "AllRoles")]
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

        [Authorize(Policy = "AllRoles")]
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
    }
}
