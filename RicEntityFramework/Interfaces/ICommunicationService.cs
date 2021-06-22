using System;
using RicModel.RoomRent;

namespace RicEntityFramework.Interfaces
{
    public interface ICommunicationService
    {
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
