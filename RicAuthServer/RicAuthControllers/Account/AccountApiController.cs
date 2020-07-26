using Microsoft.AspNetCore.Mvc;

namespace RicAuthServer.RicAuthControllers.Account
{
    public class AccountApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}