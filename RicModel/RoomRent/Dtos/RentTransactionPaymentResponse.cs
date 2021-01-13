using RicModel.RoomRent.Enumerations;
using System;

namespace RicModel.RoomRent.Dtos
{
    public class RentTransactionPaymentResponse
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime DatePaid { get; set; }
        public string DatePaidString { get; set; }
        public string paymentTransactionType { get; set; }
        public bool IsNoAdvanceDepositLeft { get; set; }
    }
}
