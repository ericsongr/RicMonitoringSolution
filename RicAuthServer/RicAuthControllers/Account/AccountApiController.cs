using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RicAuthServer.Data;
using RicAuthServer.Extensions;
using RicAuthServer.RicAuthControllers.ExtensionMethods;
using RicAuthServer.ViewModels;

namespace RicAuthServer.RicAuthControllers.Account
{
    [AllowAnonymous]
    [Route("api/account")]
    public class AccountApiController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AccountApiController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet()]
        public IActionResult Get([FromQuery]string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                var users = _context.Users.Select(ApplicationUser.Projection).ToList();

                return Ok(HandleApiSuccess(users));

            }
            else
            {
                var appUser = _context.Users
                    .FirstOrDefault(o => o.UserName.Trim().ToLower() == username.Trim().ToLower());

                var user = appUser.Projection();
                var role = _userManager.GetRolesAsync(appUser).GetAwaiter().GetResult();
                if (role.Any())
                {
                    user.Role = role.FirstOrDefault();
                }

                return Ok(HandleApiSuccess(user));

            }

        }

        [HttpPost()]
        public IActionResult Create([FromBody]UserEntryViewModel userEntryEntry)
        {
            bool success = false;
            IdentityResult result = new IdentityResult();
            if (ModelState.IsValid)
            {
                var role = userEntryEntry.Role;
                var password = "Pa$$w0rd"; //default password for new user

                var user = new ApplicationUser(userEntryEntry);

                if (_roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                {
                    result = _userManager.CreateAsync(user, password).GetAwaiter().GetResult();
                    if (result.Succeeded)
                    {
                        result = _userManager.AddToRoleAsync(user, role).GetAwaiter().GetResult();
                        if (result.Succeeded)
                            success = true;
                    }
                }
                else
                {
                    return Ok(HandleApiException("Role does not exists.", HttpStatusCode.BadRequest));
                }
                
            }

            if (success)
                return Ok(HandleApiSuccess("New user has been added."));
            else
                return Ok(HandleApiException(result.ShowErrors(), HttpStatusCode.BadRequest));
        }

        [HttpPut()]
        public IActionResult Update([FromBody]UserEntryViewModel userEntryEntry)
        {
            bool success = false;
            IdentityResult result = new IdentityResult();
            if (ModelState.IsValid)
            {
                var role = userEntryEntry.Role;

                var user = new ApplicationUser(userEntryEntry);

                if (_roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                {
                    var userModel = _userManager.FindByNameAsync(user.UserName).GetAwaiter().GetResult();
                    userModel.FirstName = userEntryEntry.FirstName;
                    userModel.LastName = userEntryEntry.LastName;
                    userModel.Email = userEntryEntry.Email;
                    userModel.MobileNumber = userEntryEntry.MobileNumber;
                    userModel.PhoneNumber = userEntryEntry.PhoneNumber;

                    result = _userManager.UpdateAsync(userModel).GetAwaiter().GetResult();
                    if (result.Succeeded)
                    {
                        var userRole = _userManager.GetRolesAsync(userModel).GetAwaiter().GetResult();
                        if (userRole.First() != userEntryEntry.Role)
                        {
                            _userManager.RemoveFromRoleAsync(userModel, userRole.First()).GetAwaiter().GetResult();

                            result = _userManager.AddToRoleAsync(userModel, role).GetAwaiter().GetResult();
                            if (result.Succeeded)
                                success = true;
                        }
                        else
                            success = true;
                    }
                }
                else
                {
                    return Ok(HandleApiException("Role does not exists.", HttpStatusCode.BadRequest));
                }

            }
            else
            {
                //TODO: error trapping
            }

            if (success)
                return Ok(HandleApiSuccess("user detail is now updated."));
            else
                return Ok(HandleApiException(result.ShowErrors(), HttpStatusCode.BadRequest));
        }

        [HttpPost("change-password", Name = "ChangePassword")]
        public IActionResult ChangePassword([FromBody]UserChangePasswordViewModel user)
        {
            if (ModelState.IsValid)
            {
                var userModel = _userManager.FindByNameAsync(user.Username).GetAwaiter().GetResult();
                if (userModel != null)
                {
                    var result = _userManager.ChangePasswordAsync(userModel, user.OldPassword, user.Password).GetAwaiter().GetResult();
                    if (!result.Succeeded)
                        return Ok(HandleApiException(result.ShowErrors(), HttpStatusCode.BadRequest));
                }
                else
                    return Ok(HandleApiException("Username does not exists.", HttpStatusCode.NotFound));
            }
            else
            {
                //TODO: error trapping
            }

            return Ok(HandleApiSuccess("Password has been changed."));
        }
    }
}