using System;
using System.Collections.Generic;
using RicModel.RoomRent.Audits;
using RicModel.RoomRent.Enumerations;

namespace RicModel.RoomRent
{
    public class RentTransactionPayment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime DatePaid { get; set; }
        public PaymentTransactionType PaymentTransactionType { get; set; }

        public int RentTransactionId { get; set; }
        public RentTransaction RentTransaction { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<AuditRentTransactionPayment> AuditRentTransactionPayments { get; set; }
    }
}
