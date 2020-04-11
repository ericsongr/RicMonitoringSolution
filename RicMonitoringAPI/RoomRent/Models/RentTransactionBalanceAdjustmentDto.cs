namespace RicMonitoringAPI.RoomRent.Models
{
    public class RentTransactionBalanceAdjustmentDto
    {
        public int TransactionId { get; set; }
        public decimal AdjustmentBalancePaymentDueAmount { get; set; }
        public string Note { get; set; }
    }
}
