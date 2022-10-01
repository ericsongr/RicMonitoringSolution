using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using RicCommon.Enumeration;
using RicCommunication.Interface;
using RicEntityFramework.RoomRent.Interfaces;
using RicMonitoringAPI.RoomRent.ViewModels;
using RicMonitoringAPI.Services.Interfaces;

namespace RicMonitoringAPI.Services
{
    public class OneSignalService : IOneSignalService
    {
        private readonly IPushNotificationGateway _pushNotificationGateway;
        private readonly ISettingRepository _settingRepository;

        public OneSignalService(
            IPushNotificationGateway pushNotificationGateway, 
            ISettingRepository settingRepository)
        {
            _pushNotificationGateway = pushNotificationGateway ?? throw new ArgumentNullException(nameof(pushNotificationGateway));
            _settingRepository = settingRepository ?? throw new ArgumentNullException(nameof(settingRepository));
        }

        public List<UserRegisteredDeviceApiModel> GetUserRegisteredDevices(string registeredDevicesJsonString)
        {
            var userRegisteredDeviceDeserializeObject =
                JsonSerializer.Deserialize(registeredDevicesJsonString,
                    typeof(List<UserRegisteredDeviceDeserializeObject>)) as List<UserRegisteredDeviceDeserializeObject>;

            var userRegisteredDevices = (from d in userRegisteredDeviceDeserializeObject
                group d by d.AspNetUsersId
                into g
                select new UserRegisteredDeviceApiModel
                {
                    PortalUserId = g.Key,
                    DeviceIds = g.Select(o => o.DeviceId).ToList()
                }).ToList();
            return userRegisteredDevices;
        }

        public void SendPushNotification(DateTime currentDateTimeUtc, string registeredDevicesJsonString, string message, string title)
        {
            var userRegisteredDevices = GetUserRegisteredDevices(registeredDevicesJsonString);
            userRegisteredDevices.ForEach(user =>
            {
                _pushNotificationGateway.SendNotification(user.PortalUserId, user.DeviceIds, title, message);
            });
        }
    }
}
