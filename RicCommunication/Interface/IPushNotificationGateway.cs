using System;
using System.Collections.Generic;
using System.Text;
using RicCommunication.PushNotification;

namespace RicCommunication.Interface
{
    public interface IPushNotificationGateway
    {
        bool SendNotification(string portalUserId, List<string> devicesIds, string title, string message);
        bool SendNotification(IList<string> portalUserIds, List<string> devicesIds, string title, string message);
        OneSignalDeviceInfo IsDeviceIdValid(string deviceId);
        bool UpdateDeviceExternalUserId(string deviceId, string externalUserId);
    }
}
