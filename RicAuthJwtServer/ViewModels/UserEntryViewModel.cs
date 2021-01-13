using System.ComponentModel.DataAnnotations;
using RicAuthJwtServer.Data;

namespace RicAuthJwtServer.ViewModels
{
    public class UserEntryViewModel : ApplicationUser
    {
        [Required(ErrorMessage = "Please select role.")]
        public string Role { get; set; }
    }
}
