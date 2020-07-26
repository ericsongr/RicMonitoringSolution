using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using RicAuthServer.ViewModels;

namespace RicAuthServer.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        { }

        public ApplicationUser(UserViewModel userEntry)
        {
            UserName = userEntry.UserName;
            FirstName = userEntry.FirstName;
            LastName = userEntry.LastName;
            Email = userEntry.Email;
            MobileNumber = userEntry.MobileNumber;
            PhoneNumber = userEntry.PhoneNumber;
        }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(20)]
        public string MobileNumber { get; set; }

    }
}
