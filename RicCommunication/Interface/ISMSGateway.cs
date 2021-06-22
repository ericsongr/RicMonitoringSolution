using System.Collections.Generic;
using RicCommunication.Model;

namespace RicCommunication.Interface
{
    public interface ISMSGateway
    {
        bool SendSMS(string fromNumber, string toNumber, string text);

        string SendSMSv2(string fromNumber, string toNumber, string text);

        IList<SmsMessage> GetReplies();

        bool DeleteReply(string messageId);

        string ProviderName { get; }
    }
}
