using System.ComponentModel.DataAnnotations;
using RicAuthServer.Data;

namespace RicAuthServer.ViewModels
{
    public class UserEntryViewModel : ApplicationUser
    {
        [Required(ErrorMessage = "Please select role.")]
        public string Role { get; set; }
    }
}
