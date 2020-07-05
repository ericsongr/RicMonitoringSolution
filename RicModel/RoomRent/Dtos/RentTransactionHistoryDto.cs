using System;

namespace RicModel.RoomRent.Dtos
{
    public class RentTransactionHistoryDto
    {
        public string DueDateString { get; set; }
        public string PaidOrUsedDepositDateString { get; set; }
        public string Period { get; set; }
        public decimal? PaidAmount { get; set; }
        public DateTime? BalanceDateToBePaid { get; set; }
        public decimal MonthlyRent { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal TotalAmountDue { get; set; }
        public bool IsDepositUsed { get; set; }
        public string Note { get; set; }
        public string TransactionType { get; set; }
    }
}
