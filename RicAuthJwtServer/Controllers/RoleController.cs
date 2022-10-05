using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using RicAuthJwtServer.ViewModels;

namespace RicAuthJwtServer.Controllers
{
    [EnableCors("AllowCors")]
    [Route("api/user-role")]
    [Authorize(Policy = "AllRoles")]
    [ApiController]
    public class RoleController : ApiJwtBaseController
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        [HttpGet]
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.Where(o => o.Name !=  UserRoles.RunDailyBatch)
                .Select(o => new UserRoleViewModel{Id = o.Id, Name = o.Name})
                .ToList();

            return Ok(new BaseRestApiModel
            {
                Payload = roles
            });
        }
        
    }
}
