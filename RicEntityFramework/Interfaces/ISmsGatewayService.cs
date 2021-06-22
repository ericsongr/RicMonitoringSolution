using System.Collections.Generic;
using RicCommunication.Interface;
using RicCommunication.Model;

namespace RicEntityFramework.Interfaces
{
    public interface ISmsGatewayService
    {
        bool DeleteReply(string messageId);
        IList<SmsMessage> GetReplies();
        ISMSGateway GetSmsGateway();
        bool SendSms(string fromNumber, string toNumber, string text);
        string SendSmsV2(string fromNumber, string toNumber, string text);
    }
}
