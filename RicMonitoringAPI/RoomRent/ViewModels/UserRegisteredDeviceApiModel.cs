
using System.Collections.Generic;

namespace RicMonitoringAPI.RoomRent.ViewModels
{
    public class UserRegisteredDeviceDeserializeObject
    {
        public string AspNetUsersId { get; set; }
        public string DeviceId { get; set; }
    }

    public class UserRegisteredDeviceApiModel
    {
        public string PortalUserId { get; set; }
        public List<string> DeviceIds { get; set; }
    } 
   
}
