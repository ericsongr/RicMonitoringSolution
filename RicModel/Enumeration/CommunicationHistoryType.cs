using System;

namespace RicModel.Enumeration
{
    [Serializable]
    public enum CommunicationHistoryType
    {
        SMS = 1,
        Email = 2,
        MMS = 3,
        PushNotifications = 4,
        SmsReply = 5
    }
}
