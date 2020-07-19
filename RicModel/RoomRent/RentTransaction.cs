using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using RicModel.Enumeration;
using RicModel.RoomRent.Audits;

namespace RicModel.RoomRent
{
    public class RentTransaction
    {
        public int Id { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public string Period { get; set; }
        public decimal? PaidAmount { get; set; }
        public DateTime? BalanceDateToBePaid { get; set; }
        public decimal? Balance { get; set; }
        public decimal ExcessPaidAmount { get; set; }
        public decimal TotalAmountDue { get; set; }
        public string Note { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }

        public int RoomId { get; set; }
        public int RenterId { get; set; }

        public bool IsSystemProcessed { get; set; }
        public DateTime? SystemDateTimeProcessed { get; set; }
        public bool IsProcessed { get; set; }

        public virtual Room Room { get; set; }
        public virtual Renter Renter { get; set; }

        public ICollection<RentTransactionDetail> RentTransactionDetails { get; set; }
        public ICollection<RentArrear> RentArrears { get; set; }
        public ICollection<RentTransactionPayment> RentTransactionPayments { get; set; }

        //audit tables
        public ICollection<AuditRentTransaction> AuditRentTransactions { get; set; }

        [NotMapped]
        public bool IsDepositUsed { get; set; }

        public string DueDateString => DueDate.ToString("dd-MMM-yyyy");

        public string PaidDateString => PaidDate?.ToString("dd-MMM-yyyy");


    }
}
