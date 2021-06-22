using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RicModel.RoomRent.Audits;

namespace RicModel.RoomRent
{
    public class Renter
    {
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

        public string Email { get; set; }
        public bool EmailRenterBeforeDueDateEnable { get; set; }

        public string Mobile { get; set; }
        public bool MobileRenterBeforeDueDateEnable { get; set; }

        public DateTime? DateEndRent { get; set; }
        public int NoOfPersons { get; set; }
        public int RoomId { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal? BalanceAmount { get; set; }
        public DateTime? BalancePaidDate { get; set; }
        public DateTime PreviousDueDate { get; set; }
        public DateTime NextDueDate { get; set; }

        public virtual Room Room { get; set; }

        public virtual ICollection<RentTransaction> RentTransactions { get; set; }
        public virtual ICollection<RentArrear> RentArrears { get; set; }

        //audits
        public virtual ICollection<AuditRenter> AuditRenters { get; set; }
        public virtual ICollection<AuditRentTransaction> AuditRentTransactions { get; set; }
        public virtual ICollection<RenterCommunicationHistory> RenterCommunicationHistories { get; set; }

    }
}
