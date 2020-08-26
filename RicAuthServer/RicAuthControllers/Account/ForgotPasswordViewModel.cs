using System.ComponentModel.DataAnnotations;

namespace RicAuthServer.RicAuthControllers.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
