using System.ComponentModel.DataAnnotations;
using RicAuthServer.Data;

namespace RicAuthServer.ViewModels
{
    public class UserViewModel : ApplicationUser
    {
        public string CurrentPassword { get; set; }
        public string Password { get; set; }

        [Required(ErrorMessage = "Please select role.")]
        public string Role { get; set; }
    }
}
