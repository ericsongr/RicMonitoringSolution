using System;
using System.Collections.Generic;

namespace RicModel.RoomRent.Dtos
{
    public class RentTransactionHistoryDto
    {
        public int Id { get; set; }
        public DateTime DueDate { get; set; }
        public string DueDateString { get; set; }
        public string PaidOrUsedDepositDateString { get; set; }
        public string Period { get; set; }
        public decimal? PaidAmount { get; set; }
        public string BalanceDateToBePaidString { get; set; }
        public decimal MonthlyRent { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal TotalAmountDue { get; set; }
        public bool IsDepositUsed { get; set; }
        public string Note { get; set; }
        public string TransactionType { get; set; }

        public List<RentTransactionHistoryPaymentDetailDto> Payments { get; set; }
    }
}
