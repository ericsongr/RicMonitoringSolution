using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RicAuthServer.Controllers
{
    [Route("UserManagement")]
    public class UserManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,Superuser")]
        [Route("Admin")]
        public IActionResult Administrator()
        {
            return Ok("Administrator, Superuser");
        }

        [HttpGet]
        [Route("Super")]
        [Authorize(Roles = "Superuser")]
        public IActionResult Superuser()
        {
            return Ok("Superuser");
        }
    }
}