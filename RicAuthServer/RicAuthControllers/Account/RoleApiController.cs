using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RicAuthServer.Data;
using RicAuthServer.ViewModels;

namespace RicAuthServer.RicAuthControllers.Account
{
    [AllowAnonymous]
    [Route("api/role")]
    public class RoleApiController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public RoleApiController(
            ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet()]
        public IActionResult Get()
        {
            var roles = _context.Roles.Select(role => new RoleViewModel
            {
                RoleName = role.Name,
            }).ToList();

            return Ok(HandleApiSuccess(roles));
        }
    }
}