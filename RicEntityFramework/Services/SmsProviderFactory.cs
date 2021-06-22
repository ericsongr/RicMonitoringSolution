using RicCommunication.Interface;
using RicCommunication.SmsGateway;
using RicModel.RoomRent;

namespace RicEntityFramework.Services
{
    public static class SmsProviderFactory
    {
        public static ISMSGateway GetProvider(SmsGateway sms)
        {
            return new SMSGlobal(sms.UserName, sms.Password, sms.GatewayUrl);
            //switch (sms.Name)
            //{
            //    case "Message Media":
            //        return new MessageMedia(sms.UserName, sms.Password);
            //    case "Sms Global Rest-API":
            //        return new SMSGlobalRestApi(sms.UserName, sms.Password, sms.GatewayUrl, sms.DedicatedNumber);
            //    case "TextLocal Messenger":
            //        return new TextLocal(sms.UserName, sms.Password, sms.GatewayUrl);
            //    case "ClickSend":
            //        return new ClickSendApi(sms.UserName, sms.Password, sms.GatewayUrl);
            //    case "Cellcast":
            //        return new Cellcast(sms.Password, sms.GatewayUrl);
            //    default:
            //        return new SMSGlobal(sms.UserName, sms.Password, sms.GatewayUrl);
            //}
        }
    }
}
