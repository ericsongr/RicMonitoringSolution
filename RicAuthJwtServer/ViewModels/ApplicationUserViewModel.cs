using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace RicAuthJwtServer.ViewModels
{
    public class ApplicationUserViewModel : IdentityUser
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MobileNumber { get; set; }

        public bool IsReceiveDueDateAlertPushNotification { get; set; }
        public bool IsPaidPushNotification { get; set; }
        public bool IsIncomingDueDatePushNotification { get; set; }
        public bool IsBatchProcessCompletedPushNotification { get; set; }

        public string Role { get; set; }

        public List<RegisteredDeviceViewModel> RegisteredDevices { get; set; }
       
    }
}
