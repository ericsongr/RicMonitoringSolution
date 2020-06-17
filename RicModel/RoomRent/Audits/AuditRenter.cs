using System;
using System.ComponentModel.DataAnnotations;

namespace RicModel.RoomRent.Audits
{
    public class AuditRenter : IAudit
    {
        public int AuditRenterId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int AdvanceMonths { get; set; }
        public int MonthsUsed { get; set; }

        [DataType(DataType.Date)]
        public DateTime AdvancePaidDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        public int DueDay { get; set; }
        public bool IsEndRent { get; set; }
        public DateTime? DateEndRent { get; set; }
        public int NoOfPersons { get; set; }
        public int RoomId { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal? BalanceAmount { get; set; }
        public DateTime? BalancePaidDate { get; set; }
        public DateTime PreviousDueDate { get; set; }
        public DateTime NextDueDate { get; set; }

        public DateTime AuditDateTime { get; set; }
        public string Username { get; set; }
        public string AuditAction { get; set; }

        public virtual Room Room { get; set; }
        public virtual Renter Renter { get; set; }

    }
}
