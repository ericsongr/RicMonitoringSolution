using RicMonitoringAPI.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RicMonitoringAPI.RoomRent.Entities
{
    public class Renter : IEntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AdvanceMonths { get; set; }
        public int MonthsUsed { get; set; }

        [DataType(DataType.Date)]
        public DateTime AdvancePaidDate { get; set; }
        public string AdvancePaidDateString { get { return AdvancePaidDate.ToShortDateString(); } }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        public string StartDateString { get { return StartDate.ToShortDateString();  } }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
        public bool IsEndRent { get; set; }
        public DateTime? DateEndRent { get; set; }
        public string DueDateString { get { return DueDate.ToShortDateString(); } }
        public int NoOfPersons { get; set; }
        public int RoomId { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public DateTime? BalancePaidDate { get; set; }
        public virtual Room Room { get; set; }

        public ICollection<RentTransaction> RentTransactions { get; set; }

    }
}
