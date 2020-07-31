using System.ComponentModel.DataAnnotations;
using RicAuthServer.Data;

namespace RicAuthServer.ViewModels
{
    public class UserEntryViewModel : ApplicationUser
    {
        public string ConfirmPassword { get; set; }
        public string Password { get; set; }

        [Required(ErrorMessage = "Please select role.")]
        public string Role { get; set; }
    }
}
