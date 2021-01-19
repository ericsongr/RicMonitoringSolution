using RicAuthJwtServer.Models;

namespace RicAuthJwtServer.Application.Interfaces
{
    public interface IRegisteredDeviceService
    {
        void Delete(string userId);
        RegisteredDevice Find(string userId, string deviceId);
        void Save(RegisteredDevice item);
        void Save(int id, string userId, string deviceId, string platform);
    }
}
