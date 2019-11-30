using RicMonitoringAPI.Services.Interfaces;
using System;

namespace RicMonitoringAPI.RoomRent.Entities
{
    public class RentTransaction : IEntityBase
    {
        public int Id { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime PaidDate { get; set; }
        public decimal Amount { get; set; }
        public decimal? Balance { get; set; }
        public DateTime? BalanceDateToBePaid { get; set; }
        public bool IsDepositUsed { get; set; }
        public string Note { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int RenterId { get; set; }
        public Renter Renter { get; set; }

    }
}
