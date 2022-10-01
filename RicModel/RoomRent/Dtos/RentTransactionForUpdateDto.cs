using System;

namespace RicModel.RoomRent.Dtos
{
    public class RentTransactionForUpdateDto
    {
        public int RenterId { get; set; }
        public int TransactionId { get; set; }
        public int RentTransactionPaymentId { get; set; }
        public bool IsDepositUsed { get; set; }
        public bool IsAddingPayment { get; set; }
        public bool IsEditingPayment { get; set; }

        public decimal? PaidAmount { get; set; }
        public string PaidDateInput { get; set; }
        public string Note { get; set; }
        public decimal TotalAmountDue { get; set; }

        public DateTime? PaidDate { get; set; }
        public DateTime? BalanceDateToBePaid { get; set; }
        public decimal? Balance { get; set; }
        
        public string BalanceDateToBePaidInput { get; set; }
        public bool IsNoAdvanceDepositLeft { get; set; }

        public string RegisteredDevicesJsonString { get; set; }
    }
}
