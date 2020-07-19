using RicModel.RoomRent.Audits;

namespace RicModel.RoomRent.Dtos.Audits
{
    public class AuditRentTransactionDto : AuditRentTransaction
    {
        public string RoomName => Room.Name;

        public string RenterName => Renter.Name;

        public string DueDateString => DueDate.ToString("dd-MMM-yyyy");

        public string PaidDateString => PaidDate?.ToString("dd-MMM-yyyy");

        public string BalanceDateToBePaidString => BalanceDateToBePaid?.ToString("dd-MMM-yyyy");

        public string DateUsedDepositString => SystemDateTimeProcessed?.ToString("dd-MMM-yyyy");

        public string SystemDateTimeProcessedString => SystemDateTimeProcessed?.ToString("dd-MMM-yyyy");

        public string AuditDateTimeString => AuditDateTime.ToString("dd-MMM-yyyy");


    }
}
