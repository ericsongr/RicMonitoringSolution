using System.ComponentModel.DataAnnotations;

namespace RicAuthJwtServer.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        public bool IsReceiveDueDateAlertPushNotification { get; set; }

    }
}
