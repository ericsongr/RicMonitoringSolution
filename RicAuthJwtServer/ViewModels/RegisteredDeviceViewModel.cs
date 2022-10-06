using System;
using System.ComponentModel.DataAnnotations;

namespace RicAuthJwtServer.ViewModels
{

    public class RegisteredDeviceViewModel
    {
        [Key]
        public long Id { get; set; }
        public string DeviceId { get; set; }
        public string Platform { get; set; }
        public string LastAccessOnUtc { get; set; }

        public bool IsReceiveDueDateAlertPushNotification { get; set; }
        public bool IsPaidPushNotification { get; set; }
        public bool IsIncomingDueDatePushNotification { get; set; }

    }
}
