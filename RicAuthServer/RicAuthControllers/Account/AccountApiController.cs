using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RicAuthServer.Data;
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

        public AccountApiController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        [HttpPost()]
        public IActionResult Create([FromBody]UserViewModel userEntry)
        {
            bool success = false;
            IdentityResult result = new IdentityResult();
            if (ModelState.IsValid)
            {
                var role = userEntry.Role;
                var password = userEntry.Password;

                var user = new ApplicationUser(userEntry);

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
        public IActionResult Update([FromBody]UserViewModel userEntry)
        {
            bool success = false;
            IdentityResult result = new IdentityResult();
            if (ModelState.IsValid)
            {
                var role = userEntry.Role;
                var password = userEntry.Password;

                var user = new ApplicationUser(userEntry);

                if (_roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                {
                    var userModel = _userManager.FindByNameAsync(user.UserName).GetAwaiter().GetResult();
                    userModel.FirstName = userEntry.FirstName;
                    userModel.LastName = userEntry.LastName;
                    userModel.Email = userEntry.Email;
                    userModel.MobileNumber = userEntry.MobileNumber;
                    userModel.PhoneNumber = userEntry.PhoneNumber;

                    result = _userManager.UpdateAsync(userModel).GetAwaiter().GetResult();
                    if (result.Succeeded)
                    {
                        var userRole = _userManager.GetRolesAsync(userModel).GetAwaiter().GetResult();
                        if (userRole.First() != userEntry.Role)
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

            if (success)
                return Ok(HandleApiSuccess("user detail is now updated."));
            else
                return Ok(HandleApiException(result.ShowErrors(), HttpStatusCode.BadRequest));
        }


    }
}