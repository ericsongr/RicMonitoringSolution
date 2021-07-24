using System;
using System.Collections.Generic;
using RicModel.Enumeration;
using RicModel.RoomRent;

namespace RicEntityFramework.Interfaces
{
    public interface ICommunicationService
    {
        List<RenterCommunicationHistory> GetRenter(int renterId, CommunicationType communicationType);

        RenterCommunicationHistory GetById(int id);

        bool SendSmsToRenter(string toNumber, string replacedText, int renterId, string batchId = null,
            bool throwException = false);

        int BillSmsFees(string text, int renterId);

        int CalculateSmsLength(string text);

        void Save(RenterCommunicationHistory comm);

        void Save(int renterId, DateTime communicationDate, int type, string destination,
            string textContent, bool isSuccessful, string batchId = null,
            string messageId = null, bool hasRead = true, string attachment = null, string contentType = null);
    }
}
