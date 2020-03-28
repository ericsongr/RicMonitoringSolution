using RicMonitoringAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using RicMonitoringAPI.Common.Enumeration;

namespace RicMonitoringAPI.RoomRent.Entities
{
    public class RentTransaction : IEntityBase
    {
        public int Id { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public string Period { get; set; }
        public decimal? PaidAmount { get; set; }
        public DateTime? BalanceDateToBePaid { get; set; }
        public decimal? Balance { get; set; }
        public decimal TotalAmountDue { get; set; }
        public bool IsDepositUsed { get; set; }
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
        public ICollection<Renter> Renters { get; set; }
        public ICollection<RentArrear> RentArrears { get; set; }

    }
}
