using System;
using System.ComponentModel.DataAnnotations;
using RicAuthJwtServer.Data;

namespace RicAuthJwtServer.Models
{
    public class RegisteredDevice
    {
        [Key]
        public long Id { get; set; }
        
        public string DeviceId { get; set; }
        public string Platform { get; set; }
        public DateTime LastAccessOnUtc { get; set; }

        public string AspNetUsersId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
