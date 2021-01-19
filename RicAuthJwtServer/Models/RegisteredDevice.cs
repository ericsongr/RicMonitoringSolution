using System;

namespace RicAuthJwtServer.Models
{
    public class RegisteredDevice
    {
        public long Id { get; set; }
        public string AspNetUsersId { get; set; }
        public string DeviceId { get; set; }
        public string Platform { get; set; }
        public DateTime LastAccessOnUtc { get; set; }
    }
}
