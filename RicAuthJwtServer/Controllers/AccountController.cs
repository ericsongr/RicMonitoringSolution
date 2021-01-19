using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RicAuthJwtServer.Data;
using RicAuthJwtServer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RicAuthJwtServer.Application;
using RicAuthJwtServer.Application.Interfaces;
using RicAuthJwtServer.Infrastructure;
using RicCommon.Diagnostics;
using RicMonitoringAPI.Common.Model;
using BaseRestApiModel = RicAuthJwtServer.ViewModels.BaseRestApiModel;

namespace RicAuthJwtServer.Controllers
{
    [EnableCors("AllowCors")]
    [Route("api/account")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IRegisteredDeviceService _registeredDeviceService;
        private readonly IAspNetUserLoginTokenService _aspNetUserLoginTokenService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IRefreshTokenService refreshTokenService,
            IRegisteredDeviceService registeredDeviceService,
            IAspNetUserLoginTokenService aspNetUserLoginTokenService
            )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;

            _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
            _registeredDeviceService = registeredDeviceService ?? throw new ArgumentNullException(nameof(registeredDeviceService));
            _aspNetUserLoginTokenService = aspNetUserLoginTokenService ?? throw new ArgumentNullException(nameof(aspNetUserLoginTokenService));
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExist = await userManager.FindByNameAsync(model.UserName);
            if (userExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed." });
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully." });
        }

        [HttpPost]
        [Route("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExist = await userManager.FindByNameAsync(model.UserName);
            if (userExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed." });
            }

            if (!await roleManager.RoleExistsAsync(UserRoles.Administrator))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Administrator));

            if (!await roleManager.RoleExistsAsync(UserRoles.Superuser))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Superuser));

            if (!await roleManager.RoleExistsAsync(UserRoles.Staff))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Staff));

            if (await roleManager.RoleExistsAsync(UserRoles.Administrator))
                await userManager.AddToRoleAsync(user, UserRoles.Administrator);


            return Ok(new Response { Status = "Success", Message = "User created successfully." });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginInputModel loginUser)
        {
            var loginUserResult = await userManager.FindByNameAsync(loginUser.Username);
            if (loginUserResult == null)
            {
                string errorMessage = $"The user name is incorrect - {loginUser.Username}";
                return Ok(HandleApiException(errorMessage, HttpStatusCode.BadRequest));
            }

            if (loginUserResult != null && await userManager.CheckPasswordAsync(loginUserResult, loginUser.Password))
            {
                if (!string.IsNullOrEmpty(loginUser.DeviceId))
                {
                    var device = _registeredDeviceService.Find(loginUserResult.Id, loginUser.DeviceId);
                    if (device == null)
                    {
                        _registeredDeviceService.Save(0, loginUserResult.Id, loginUser.DeviceId, loginUser.Platform);
                    }
                    else
                    {
                        device.LastAccessOnUtc = DateTime.UtcNow;
                        _registeredDeviceService.Save(device);
                    }
                }

                var role = "";
                var userRoles = await userManager.GetRolesAsync(loginUserResult);
                foreach (var userRole in userRoles)
                    role = userRole;
                
                var issuer = _configuration["JWT:ValidIssuer"];
                var audienceId = _configuration["JWT:ValidAudience"];
                var secret = _configuration["JWT:Secret"];
                var timeoutInMinutes = 60; // 60 minutes
                var name = $"{loginUserResult.FirstName} {loginUserResult.LastName}";

                var jwtFormat = new JwtFormat(issuer);
                var accessToken = jwtFormat.GenerateAccessToken(audienceId, secret, loginUserResult.Email, timeoutInMinutes, loginUserResult.Id, name, role);
                var refreshToken = _refreshTokenService.GenerateRefreshToken(loginUserResult.Id, loginUser.DeviceId);
                string loginToken = _aspNetUserLoginTokenService.GenerateLoginToken(loginUserResult.Id);

                return Ok(new BaseRestApiModel
                {
                    Payload = new
                    {
                        userId = loginUserResult.Id,
                        name,
                        role,
                        accessToken,
                        accessTokenExpiresIn = timeoutInMinutes,
                        refreshToken = refreshToken.FirstOrDefault().Key,
                        refreshTokenExpiresIn = refreshToken.First().Value,
                        loginToken
                        
                    },
                    Errors = new List<BaseErrorModel>(),
                    StatusCode = (int)HttpStatusCode.OK
                });
            }
            else
            {
                string errorMessage = $"The password is incorrect.";
                return Ok(HandleApiException(errorMessage, HttpStatusCode.BadRequest));
            }

        }

        [Route("validate")]
        public IActionResult PostRefreshToken(RefreshTokenApiModel token)
        {
            try
            {
                if (string.IsNullOrEmpty(token.Xoken))
                    return Ok(HandleApiException("Refresh token is invalid or had expired.", HttpStatusCode.BadRequest));

                var validatedToken = _refreshTokenService.IsRefreshTokenValid(token.Xoken);

                if (validatedToken == null)
                {
                    Logger.Write($"Invalid refresh token {token.Xoken}");
                    return Ok(HandleApiException("Refresh token is invalid or had expired.", HttpStatusCode.BadRequest));
                }

                var user = userManager.FindByIdAsync(validatedToken.UserId).GetAwaiter().GetResult();
                if (user == null)
                {
                    var message = $"User Id does not exists {validatedToken.UserId}";
                    Logger.Write(message);
                    return Ok(HandleApiException(message, HttpStatusCode.BadRequest));
                }

                var device = _registeredDeviceService.Find(validatedToken.UserId, validatedToken.DeviceId);
                if (device != null)
                {
                    device.LastAccessOnUtc = DateTime.UtcNow;
                    _registeredDeviceService.Save(device);
                }
                else
                {
                    Logger.Write($"Device not found {validatedToken.UserId} - {validatedToken.DeviceId}");
                }

                var role = "";
                var userRoles = userManager.GetRolesAsync(user).GetAwaiter().GetResult();
                foreach (var userRole in userRoles)
                    role = userRole;

                var issuer = _configuration["JWT:ValidIssuer"];
                var audienceId = _configuration["JWT:ValidAudience"];
                var secret = _configuration["JWT:Secret"];
                var timeoutInMinutes = 60; // 60 minutes
                var name = $"{user.FirstName} {user.LastName}";

                var jwtFormat = new JwtFormat(issuer);
                string accessToken = jwtFormat.GenerateAccessToken(audienceId, secret, user.Email, timeoutInMinutes, user.Id, name, role);
                var refreshToken = _refreshTokenService.GenerateRefreshToken(validatedToken.UserId, validatedToken.DeviceId);
                string loginToken = _aspNetUserLoginTokenService.GetLoginToken(validatedToken.UserId)?.TokenValue ?? "";

                return Ok(new BaseRestApiModel
                {
                    Payload = new
                    {
                        userId = user.Id,
                        name,
                        role,
                        accessToken,
                        accessTokenExpiresIn = timeoutInMinutes,
                        refreshToken = refreshToken.FirstOrDefault().Key,
                        refreshTokenExpiresIn = refreshToken.First().Value,
                        loginToken
                    },
                    Errors = new List<BaseError>(),
                    StatusCode = (int)HttpStatusCode.OK
                });

            }
            catch (Exception ex)
            {
                Logger.Write(ex, LoggerLevel.Fatal);
                return Ok(HandleApiException(ex.ToString(), HttpStatusCode.InternalServerError));
            }
        }

        #region Helpers

        private BaseRestApiModel HandleApiException(string message, HttpStatusCode httpStatusCode)
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

        #endregion

        #region Private Functions

        private bool DeleteMember(ApplicationUser user)
        {
            _refreshTokenService.Delete(user.Id);
            _registeredDeviceService.Delete(user.Id);
            _aspNetUserLoginTokenService.Delete(user.Id);
            if (user != null)
            {
                var result = userManager.DeleteAsync(user).Result;
            }
            return true;
        }

        #endregion
    }
}
