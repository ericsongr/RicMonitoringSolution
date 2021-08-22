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
using System.Text;
using System.Threading.Tasks;
using RicAuthJwtServer.Application.Interfaces;
using RicAuthJwtServer.Data.Extensions;
using RicAuthJwtServer.Data.Services;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IRegisteredDeviceService _registeredDeviceService;
        private readonly IAspNetUserLoginTokenService _aspNetUserLoginTokenService;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IRefreshTokenService refreshTokenService,
            IRegisteredDeviceService registeredDeviceService,
            IAspNetUserLoginTokenService aspNetUserLoginTokenService,
            IEmailSender emailSender,
            IConfiguration config
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;

            _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
            _registeredDeviceService = registeredDeviceService ?? throw new ArgumentNullException(nameof(registeredDeviceService));
            _aspNetUserLoginTokenService = aspNetUserLoginTokenService ?? throw new ArgumentNullException(nameof(aspNetUserLoginTokenService));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExist = await _userManager.FindByNameAsync(model.UserName);
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

            var result = await _userManager.CreateAsync(user, model.Password);
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
            var userExist = await _userManager.FindByNameAsync(model.UserName);
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

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed." });
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Administrator))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Administrator));

            if (!await _roleManager.RoleExistsAsync(UserRoles.Superuser))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Superuser));

            if (!await _roleManager.RoleExistsAsync(UserRoles.Staff))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Staff));

            if (await _roleManager.RoleExistsAsync(UserRoles.Administrator))
                await _userManager.AddToRoleAsync(user, UserRoles.Administrator);


            return Ok(new Response { Status = "Success", Message = "User created successfully." });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginInputModel loginUser)
        {
            var loginUserResult = await _userManager.FindByNameAsync(loginUser.Username);
            if (loginUserResult == null)
            {
                string errorMessage = $"The user name is incorrect - {loginUser.Username}";
                return Ok(HandleApiException(errorMessage, HttpStatusCode.BadRequest));
            }

            if (loginUserResult != null && await _userManager.CheckPasswordAsync(loginUserResult, loginUser.Password))
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
                var userRoles = await _userManager.GetRolesAsync(loginUserResult);
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
                        portalUserId = loginUserResult.Id,
                        username = loginUser.Username,
                        name,
                        role,
                        accessToken,
                        accessTokenExpiresIn = timeoutInMinutes,
                        refreshToken = refreshToken.FirstOrDefault().Key,
                        refreshTokenExpiresIn = refreshToken.First().Value,
                        loginToken,
                        userType = 1 //TODO fetch value from db

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

                var user = _userManager.FindByIdAsync(validatedToken.UserId).GetAwaiter().GetResult();
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
                var userRoles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult();
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

        #region Forgot & Reset Password

        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (model.Password != model.PasswordConfirm)
            {
                return Ok(HandleApiException("The password and confirm password do not match.", HttpStatusCode.BadRequest));
            }

            var user = _userManager.FindByEmailAsync(model.Email).GetAwaiter().GetResult();
            if (user == null)
            {
                return Ok(HandleApiException("Please verify you email, it seems not exists.", HttpStatusCode.BadRequest));
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Ok(new BaseRestApiModel
                {
                    Payload = "Your password has been reset.",
                    Errors = new List<BaseError>(),
                    StatusCode = (int)HttpStatusCode.OK
                });
            }
            else
            {
                var errors = ShowErrors(result);

                //something wrong if still continue to this line
                return Ok(HandleApiException(errors, HttpStatusCode.BadRequest));
            }
        }

        

        [HttpPost]
        [AllowAnonymous]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {

            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return Ok(HandleApiException("Email does not exists.", HttpStatusCode.BadRequest));
                }

                var host = _config.GetValue<string>("ClientUrl");
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Email, token, Request.Scheme, host);
                await _emailSender.SendResetPasswordAsync(model.Email, callbackUrl);

                return Ok(new BaseRestApiModel
                {
                    Payload = "Password reset link has been sent to you email.",
                    Errors = new List<BaseError>(),
                    StatusCode = (int)HttpStatusCode.OK
                });


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        [Authorize(Roles = "AllRoles")]
        [HttpPost("change-password", Name = "ChangePassword")]
        public IActionResult ChangePassword([FromBody] UserChangePasswordViewModel user)
        {
            if (ModelState.IsValid)
            {
                var userModel = _userManager.FindByNameAsync(user.Username).GetAwaiter().GetResult();
                if (userModel != null)
                {
                    var result = _userManager.ChangePasswordAsync(userModel, user.OldPassword, user.Password).GetAwaiter().GetResult();
                    if (!result.Succeeded)
                        return Ok(HandleApiException(ShowErrors(result), HttpStatusCode.BadRequest));
                }
                else
                    return Ok(HandleApiException("Username does not exists.", HttpStatusCode.NotFound));
            }
            else
            {
                return Ok(HandleApiException("Invalid form values. Please verify.", HttpStatusCode.NotFound));
            }

            return Ok(new BaseRestApiModel
            {
                Payload = "Password has been changed.",
                Errors = new List<BaseError>(),
                StatusCode = (int)HttpStatusCode.OK
            });
        }

        #endregion

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

        private string ShowErrors(IdentityResult result)
        {
            var errors = new StringBuilder();
            foreach (var error in result.Errors)
                errors.AppendLine(error.Description);
            return errors.ToString();
        }

        private bool DeleteMember(ApplicationUser user)
        {
            _refreshTokenService.Delete(user.Id);
            _registeredDeviceService.Delete(user.Id);
            _aspNetUserLoginTokenService.Delete(user.Id);
            if (user != null)
            {
                var result = _userManager.DeleteAsync(user).Result;
            }
            return true;
        }

        #endregion
    }
}
