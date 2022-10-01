using System.Collections.Generic;
using RicAuthJwtServer.Models;

namespace RicAuthJwtServer.Application.Interfaces
{
    public interface IRegisteredDeviceService
    {
        void Delete(string userId);
        RegisteredDevice Find(string userId, string deviceId);
        List<RegisteredDevice> FindReceiveDueDatePushNotifications();
        List<RegisteredDevice> FindIsPaidPushNotifications();
        void Save(RegisteredDevice item);
        void Save(int id, string userId, string deviceId, string platform);
    }
}
