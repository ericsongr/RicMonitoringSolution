using System;
using System.Collections.Generic;
using RicMonitoringAPI.RoomRent.ViewModels;

namespace RicMonitoringAPI.Services.Interfaces
{
    public interface IOneSignalService
    {
        List<UserRegisteredDeviceApiModel> GetUserRegisteredDevices(string registeredDevicesJsonString);

        void SendPushNotification(DateTime currentDateTimeUtc, string registeredDevicesJsonString, string message, string title);

    }
}
