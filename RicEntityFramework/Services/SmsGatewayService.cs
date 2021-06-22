using System;
using System.Collections.Generic;
using System.Linq;
using RicCommon.Enumeration;
using RicCommunication.Interface;
using RicCommunication.Model;
using RicEntityFramework.Interfaces;
using RicEntityFramework.RoomRent.Interfaces;

namespace RicEntityFramework.Services
{
    public class SmsGatewayService : ISmsGatewayService
    {
        private readonly ISmsGatewayRepository _smsGatewayRepository;
        private readonly ISettingRepository _settingRepository;

        public SmsGatewayService(
            ISmsGatewayRepository smsGatewayRepository,
            ISettingRepository settingRepository)
        {
            _smsGatewayRepository = smsGatewayRepository ?? throw new ArgumentNullException(nameof(smsGatewayRepository));
            _smsGatewayRepository = smsGatewayRepository;
            _settingRepository = settingRepository ?? throw new ArgumentNullException(nameof(settingRepository));
        }
        public bool DeleteReply(string messageId)
        {
            return GetSmsGateway().DeleteReply(messageId);
        }

        public IList<SmsMessage> GetReplies()
        {
            return GetSmsGateway().GetReplies();
        }

        public ISMSGateway GetSmsGateway()
        {
            var sms = _smsGatewayRepository.GetSingleAsync(x => x.IsActive).GetAwaiter().GetResult(); //should only once active sms gateway
            var useSystemDedicatedNumber = bool.Parse(_settingRepository.Get(SettingNameEnum.UseSystemDedicatedNumber).Value);
            string dedicatedNumber = useSystemDedicatedNumber
                ? _settingRepository.Get(SettingNameEnum.SMSGatewaySenderId).Value
                : sms.DedicatedNumber;
            sms.DedicatedNumber = dedicatedNumber;

            return SmsProviderFactory.GetProvider(sms);
        }

        public bool SendSms(string fromNumber, string toNumber, string text)
        {
            return GetSmsGateway().SendSMS(fromNumber, toNumber, text);
        }

        public string SendSmsV2(string fromNumber, string toNumber, string text)
        {
            return GetSmsGateway().SendSMSv2(fromNumber, toNumber, text);
        }

    }
}
