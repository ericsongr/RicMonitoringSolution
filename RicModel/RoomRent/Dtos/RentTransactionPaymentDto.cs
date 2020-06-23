using System;
using RicModel.RoomRent.Enumerations;

namespace RicModel.RoomRent.Dtos
{
    public class RentTransactionPaymentDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string DatePaidString { get; set; }
        public DateTime DatePaid { get; set; }
        public string PaymentTransactionType { get; set; }
    }
}
