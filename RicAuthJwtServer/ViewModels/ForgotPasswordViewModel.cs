using System.ComponentModel.DataAnnotations;

namespace RicAuthJwtServer.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
