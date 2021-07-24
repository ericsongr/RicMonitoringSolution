using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Mustache;
using RicCommon.Diagnostics;
using RicCommon.Enumeration;
using RicCommunication.Interface;
using RicEntityFramework.Interfaces;
using RicEntityFramework.RoomRent.Interfaces;
using RicModel.Enumeration;
using RicModel.RoomRent;
using RicModel.RoomRent.Enumerations;

namespace RicEntityFramework.Services
{
    public class CommunicationService : ICommunicationService
    {
        private readonly IRenterRepository _renterRepository;
        private readonly ISMSGateway _smsGateway;
        private readonly ISettingRepository _settingRepository;
        private readonly IRenterCommunicationRepository _renterCommunicationRepository;
        private readonly IAccountBillingItemRepository _accountBillingItemRepository;

        public CommunicationService(
            IRenterRepository renterRepository,
            ISMSGateway smsGateway,
            ISettingRepository settingRepository,
            IRenterCommunicationRepository renterCommunicationRepository,
            IAccountBillingItemRepository accountBillingItemRepository)
        {
            _renterRepository = renterRepository ?? throw new ArgumentNullException(nameof(renterRepository));
            _smsGateway = smsGateway ?? throw new ArgumentNullException(nameof(smsGateway));
            _settingRepository = settingRepository ?? throw new ArgumentNullException(nameof(settingRepository));
            _renterCommunicationRepository = renterCommunicationRepository ?? throw new ArgumentNullException(nameof(renterCommunicationRepository));
            _accountBillingItemRepository = accountBillingItemRepository ?? throw new ArgumentNullException(nameof(accountBillingItemRepository));
        }

        public List<RenterCommunicationHistory> GetRenter(int renterId, CommunicationType communicationType)
        {
            return _renterCommunicationRepository
                .FindAll()
                .Where(o => o.RenterId == renterId && o.CommunicationType == (int) communicationType)
                .AsNoTracking()
                .ToList();
        }

        public RenterCommunicationHistory GetById(int id)
        {
            return _renterCommunicationRepository
                .FindAll()
                .FirstOrDefault(o => o.Id == id);
        }

        public bool SendSmsToRenter(string toNumber, string replacedText, int renterId, string batchId = null, bool throwException = false)
        {
            if (!IsValidSmsNumber(toNumber))
            {
                //Logger.Write($"{toNumber} is not a valid mobile number", LoggerLevel.Error);
                return false;
            }

            var smsSentSuccessfully = false;

            try
            {
                if (!IsSmsServiceEnabled()) return false;

                var from = _settingRepository.GetValue(SettingNameEnum.SMSGatewaySenderId);

                if (renterId == 0)
                {
                    toNumber = EnsureNumberHasDialingCode(toNumber, renterId, GetDialCodeFromMember);

                    //var messageId = _smsGateway.SendSMSv2(from, toNumber, replacedText);
                    var messageId = Guid.NewGuid().ToString();
                    if (!string.IsNullOrWhiteSpace(messageId))
                    {
                        BillSmsFees(replacedText, renterId);

                        smsSentSuccessfully = true;

                        Logger.Write($"Sent Group TestSMS to member {renterId}-{toNumber}-{replacedText}");
                    }

                }
                else
                {
                    toNumber = EnsureNumberHasDialingCode(toNumber, renterId, GetDialCodeFromMember);

                    var messageId = _smsGateway.SendSMSv2(from, toNumber, replacedText);
                    //var messageId = Guid.NewGuid().ToString();
                    if (messageId.Contains("ERROR:"))
                    {
                        //_memberCommunicationService.LogFailedSending(renterId, CommunicationType.SMS);
                    }
                    else if (!string.IsNullOrWhiteSpace(messageId))
                    {
                        BillSmsFees(replacedText, renterId);

                        Save(renterId, DateTime.UtcNow, (int)CommunicationType.SMS, toNumber, replacedText, true, batchId, messageId, true);

                        smsSentSuccessfully = true;

                        //Logger.Write($"Sent SMS to member {renterId}-{toNumber}-{replacedText}");
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, $"Error sending SMS to member {renterId}-{toNumber}");
                if (throwException) throw;
            }

            return smsSentSuccessfully;
        }

        public int BillSmsFees(string text, int renterId)
        {
            int totalSmsBill = CalculateSmsLength(text);

            AddSmsFee(renterId, totalSmsBill);

            return totalSmsBill;
        }

        public int CalculateSmsLength(string text)
        {
            if (IsEnglishText(text))
            {
                return text.Length <= 160 ? 1 : Convert.ToInt32(Math.Ceiling(Convert.ToDouble(text.Length) / 153));
            }

            return text.Length <= 70 ? 1 : Convert.ToInt32(Math.Ceiling(Convert.ToDouble(text.Length) / 67));
        }

        private void AddSmsFee(int renterId, int totalSmsBill)
        {
            var renter = _renterRepository.FindBy(o => o.Id == renterId, renter => renter.Room.Account);
            if (renter.Any())
            {
                _accountBillingItemRepository.AddSmsFee(renter.FirstOrDefault().Room.AccountId, renterId, totalSmsBill);
            }
            
        }

        public void Save(RenterCommunicationHistory comm)
        {
            _renterCommunicationRepository.Save(comm);
        }

        public void Save(int renterId, DateTime communicationDate, int type, string destination,
            string textContent, bool isSuccessful, string batchId = null,
            string messageId = null, bool hasRead = true, string attachment = null, string contentType = null)
        {
            var comm = new RenterCommunicationHistory
            {
                CommunicationUtcdateTime = communicationDate,
                CommunicationSentTo = destination,
                RenterId = renterId,
                RequestedBy = "TODO", //_currentUser.Identity.Name,
                CommunicationText = textContent,
                CommunicationType = type,
                IsSuccessfullySent = isSuccessful,
                BatchId = batchId,
                MessageId = messageId,
                HasRead = hasRead,
                AttachmentFileName = attachment,
                ContentType = contentType
            };

            Save(comm);
        }

        #region Private Functions

        private bool IsEnglishText(string text)
        {
            return Regex.IsMatch(text, @"^[\u0000-\u007F]+$");
        }

        private string GetDialCodeFromMember(int accountId)
        {
            var account = _renterRepository
                .FindBy(o => o.Id == accountId, o => o.Room.Account)
                .Select(o => o.Room.Account)
                .FirstOrDefault();
            if (account != null)
                return account.DialingCode ?? "61";

            return "61";
        }

        private static string EnsureNumberHasDialingCode(string toNumber, int renterId, Func<int, string> dialingCodeFunc)
        {
            if (!toNumber.StartsWith("+"))
            {
                string dialCode = dialingCodeFunc(renterId);
                if (string.IsNullOrEmpty(dialCode))
                    dialCode = "61";
                if (toNumber.StartsWith("0"))
                    toNumber = toNumber.Substring(1, toNumber.Length - 1);
                toNumber = $"{dialCode}{toNumber}";
            }

            return toNumber;
        }

        private bool IsSmsServiceEnabled()
        {
            var isSendSmsEnabled = _settingRepository.GetBooleanValue(SettingNameEnum.AppSMSRenterBeforeDueDateEnable);

            //Only send a SMS if SMS has been enabled
            return isSendSmsEnabled;
        }

        private static bool IsValidSmsNumber(string toNumber)
        {
            return !string.IsNullOrWhiteSpace(toNumber) && toNumber.Length > 5;
        }

        #endregion
    }
}
