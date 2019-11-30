using System;

namespace RicMonitoringAPI.RoomRent.Entities
{
    public class RentTransaction2
    {
        public int Id { get; set; }
        public int RenterId { get; set; }
        public string Renter { get; set; }
        public int RoomId { get; set; }
        public string Room { get; set; }
        public decimal MonthlyRent { get; set; }
        public DateTime DueDate { get; set; }
        public string PaidDate { get; set; }
        public decimal Amount { get; set; }
        public decimal? Balance { get; set; }
        public DateTime? BalanceDateToBePaid { get; set; }
        public bool IsDepositUsed { get; set; }
        public string Note { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
