using System.ComponentModel.DataAnnotations;

namespace RicAuthServer.ViewModels
{
    public class UserChangePasswordViewModel
    {
        public string Username { get; set; }

        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
