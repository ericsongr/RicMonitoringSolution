using RicModel.RoomRent.Audits;

namespace RicModel.RoomRent.Dtos.Audits
{
    public class AuditRentTransactionPaymentDto : AuditRentTransactionPayment
    {
        public string DatePaidString => DatePaid.ToString("dd-MMM-yyyy");

        public string AuditDateTimeString => AuditDateTime.ToString("dd-MMM-yyyy hh:mm tt");
    }
}
