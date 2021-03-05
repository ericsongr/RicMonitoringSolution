using Microsoft.AspNetCore.Mvc;
using RicAuthJwtServer.ViewModels;

namespace RicAuthJwtServer.Controllers
{
    /// <summary>
    /// this controller only use to create IUrlHelper or url for sending email for password reset
    /// call from controller "account" with action "ForgotPassword"
    /// </summary>
    [Route("auth")]
    public class AuthController : Controller
    {
        [Route("reset-password")]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            return Content("Just to have return");
        }
    }
}
