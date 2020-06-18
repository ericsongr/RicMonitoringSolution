using System;
using RicModel.RoomRent.Audits;
using RicModel.RoomRent.Enumerations;

namespace RicModel.RoomRent
{
    public class AuditRentTransactionPayment : IAudit
    {

        public int AuditRentTransactionPaymentId { get; set; }
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime DatePaid { get; set; }
        public PaymentTransactionType PaymentTransactionType { get; set; }
        public DateTime AuditDateTime { get; set; }
        public string Username { get; set; }
        public string AuditAction { get; set; }

        public RentTransactionPayment RentTransactionPayment { get; set; }
        
    }
}
